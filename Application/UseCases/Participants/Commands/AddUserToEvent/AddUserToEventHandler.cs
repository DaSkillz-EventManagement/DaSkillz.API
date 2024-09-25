using Application.Abstractions.Email;
using Application.Helper;
using Application.ResponseMessage;
using Domain.Constants.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Participant;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Participants.Commands.AddUserToEventCommand
{
    public class AddUserToEventHandler : IRequestHandler<AddUserToEventCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _sendMail;
        private readonly IUserRepository _userRepository;
        public AddUserToEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork,
            IEmailService sendMail, IUserRepository userRepository)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
            _sendMail = sendMail;
            _userRepository = userRepository;
        }
        public async Task<APIResponse> Handle(AddUserToEventCommand request, CancellationToken cancellationToken)
        {
            var capacityCheck = await _participantRepository.IsReachedCapacity(request.RegisterEventModel.EventId);
            if (capacityCheck)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.ParticipantCapacityLimitReached,
                    Data = null
                };
            }
            var isOwner = await _eventRepo.IsOwner(request.UserId, request.RegisterEventModel.EventId);
            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            isOwner = await _eventRepo.IsOwner(request.UserId, request.RegisterEventModel.EventId);

            if (isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.HostCannotRegister,
                    Data = null
                };
            }
            Participant participant = new()
            {
                UserId = request.RegisterEventModel.UserId,
                EventId = request.RegisterEventModel.EventId,
                RoleEventId = request.RegisterEventModel.RoleEventId + 1,
                CreatedAt = DateTime.Now,
                IsCheckedMail = false,
                Status = ParticipantStatus.Confirmed.ToString()
            };
            var currentEvent = await _eventRepo.GetById(request.RegisterEventModel.EventId);
            var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
            await _participantRepository.UpSert(participant);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                #region sendMail
                var user = await _userRepository.GetById(request.RegisterEventModel.UserId);
                await _sendMail.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
                {
                    EventId = request.RegisterEventModel.EventId,
                    UserId = (Guid)currentEvent.CreatedBy!,
                    Email = user!.Email,
                    RoleEventId = request.RegisterEventModel.RoleEventId,
                    FullName = user.FullName,
                    Avatar = Owner!.Avatar,
                    EventName = currentEvent?.EventName,
                    Location = currentEvent?.Location,
                    LocationAddress = currentEvent?.LocationAddress,
                    LogoEvent = currentEvent?.Image,
                    OrgainzerName = Owner.FullName,
                    StartDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.StartDate).DateTime,
                    EndDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.EndDate).DateTime,
                    Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.EndDate).DateTime),
                    Message = TicketMailConstant.MessageMail.ElementAt(request.RegisterEventModel.RoleEventId),
                    TypeButton = Utilities.GetTypeButton(request.RegisterEventModel.RoleEventId),
                });
                #endregion

                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.Created,
                    Message = MessageCommon.SavingSuccesfully,
                    Data = request.RegisterEventModel
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageCommon.SavingFailed,
                Data = request.RegisterEventModel
            };
        }
    }
}
