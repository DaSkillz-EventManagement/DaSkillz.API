using Application.Abstractions.Caching;
using Application.Abstractions.Email;
using Application.Abstractions.Payment.ZaloPay;
using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent;
using Domain.Constants.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Enum.Participant;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetOrderStatus
{
    public class GetOrderQueryQueryHandler : IRequestHandler<GetOrderStatusQuery, APIResponse>
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCaching _caching;
        private readonly IMediator _mediator;

        public GetOrderQueryQueryHandler(IZaloPayService zaloPayService,
            ITransactionRepository transactionRepository,
            IParticipantRepository participantRepository,
            ISubscriptionRepository subscriptionRepository,
            ISponsorEventRepository sponsorEventRepository,
            IUserRepository userRepository,
            IEventRepository eventRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IRedisCaching caching,
            IMediator mediator)
        {
            _zaloPayService = zaloPayService;
            _transactionRepository = transactionRepository;
            _participantRepository = participantRepository;
            _subscriptionRepository = subscriptionRepository;
            _sponsorEventRepository = sponsorEventRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _caching = caching;
            _mediator = mediator;
        }

        public async Task<APIResponse> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _zaloPayService.QueryOrderStatus(request.appTransId!);
            var exist = await _transactionRepository.GetById(request.appTransId!);

            if (exist == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = "Can't find this transaction",
                    Data = null,
                };
            }

            var returncode = Convert.ToInt32(result["return_code"]);

            
            if (returncode == 1)
            {
                exist.Zptransid = result["zp_trans_id"].ToString();
                exist.Status = (int)TransactionStatus.SUCCESS;
                if (exist.SubscriptionType == (int)PaymentType.TICKET)
                {
                    var isExistedOnEvent = await _participantRepository.IsExistedOnEvent((Guid)exist.UserId!, (Guid)exist.EventId!);

                    if (isExistedOnEvent)
                    {
                        return new APIResponse()
                        {
                            StatusResponse = HttpStatusCode.Conflict,
                            Message = MessageParticipant.ExistedOnEvent,
                            Data = null
                        };
                    }
                    Participant participant = new()
                    {
                        UserId = (Guid)exist.UserId!,
                        EventId = (Guid)exist.EventId!,
                        RoleEventId = (int)EventRole.Visitor + 1,
                        CreatedAt = DateTime.Now,
                        IsCheckedMail = false,
                        Status = ParticipantStatus.Confirmed.ToString()
                    };

                    await _participantRepository.Add(participant);

                    var currentEvent = await _eventRepository.GetById(exist.EventId);
                    var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
                    var user = await _userRepository.GetById(exist.UserId);
                    var background = _emailService.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
                    {
                        EventId = (Guid)exist.EventId,
                        UserId = (Guid)currentEvent.CreatedBy!,
                        Email = user!.Email,
                        RoleEventId = (int)EventRole.Visitor + 1,
                        FullName = user.FullName,
                        Avatar = Owner!.Avatar,
                        EventName = currentEvent?.EventName,
                        Location = currentEvent?.Location,
                        LocationUrl = currentEvent?.LocationUrl,
                        LocationAddress = currentEvent?.LocationAddress,
                        LogoEvent = currentEvent?.Image,
                        OrgainzerName = Owner.FullName,
                        StartDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.StartDate).DateTime,
                        EndDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.EndDate).DateTime,
                        Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.EndDate).DateTime),
                        Message = TicketMailConstant.MessageMail.ElementAt((int)EventRole.Visitor),
                        TypeButton = Utilities.GetTypeButton((int)EventRole.Visitor + 1),
                    });
                }
                else if (exist.SubscriptionType == (int)PaymentType.SUBSCRIPTION)
                {
                    var subscription = await _subscriptionRepository.GetByUserId(exist.UserId);
                    if (subscription != null)
                    {
                        if (subscription.EndDate >= DateTime.UtcNow)
                        {
                            subscription.EndDate = subscription.EndDate.AddMonths(1);
                        }
                        else
                        {
                            subscription.StartDate = DateTime.UtcNow;
                            subscription.EndDate = DateTime.UtcNow.AddMonths(1);
                        }
                        subscription.IsActive = true;
                    }
                    else
                    {
                        subscription = new Subscription
                        {
                            UserId = (Guid)exist.UserId!,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMonths(1),
                            IsActive = true
                        };
                        await _subscriptionRepository.Add(subscription);
                    }
                }
                else if (exist.SubscriptionType == (int)PaymentType.SPONSOR)
                {
                    var sponsor = await _sponsorEventRepository.CheckSponsoredEvent((Guid)exist.EventId!, (Guid)exist.UserId!);
                    if (sponsor == null)
                    {
                        return new APIResponse
                        {
                            StatusResponse = HttpStatusCode.NotFound,
                            Message = "Can't find this sponsor event",
                            Data = null,
                        };
                    }
                    sponsor.IsSponsored = true;
                }
                else if (exist.SubscriptionType == (int)PaymentType.ADVERTISE)
                {
                    var numOfDate = await _caching.GetAsync<int?>($"numOfDate_{request.appTransId}");
                    await _mediator.Send(new UseAdvertisedEventQuery((Guid)exist.EventId!, (Guid)exist.UserId!, numOfDate ?? 3), cancellationToken);
                }

                await _caching.RemoveAsync($"payment_{request.appTransId}");
            }
            else if (returncode == 2)
            {
                exist.Zptransid = result["zp_trans_id"].ToString();
                exist.Status = (int)TransactionStatus.FAIL;
                await _caching.RemoveAsync($"payment_{request.appTransId}");
            }
            await _transactionRepository.Update(exist);
            await _unitOfWork.SaveChangesAsync();




            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = (string)result["return_message"],
                Data = result,
            };
        }
    }
}
