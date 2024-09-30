using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Medallion.Threading;
using MediatR;
using StackExchange.Redis;
using System.Net;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePayment, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IDistributedLockProvider _synchronizationProvider;
        private readonly IZaloPayService _zaloPayService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCaching _caching;

        public CreatePaymentHandler(ITransactionRepository transactionRepository, IUserRepository userRepository, IParticipantRepository participantRepository, IEventRepository eventRepository, IDistributedLockProvider synchronizationProvider, IZaloPayService zaloPayService, IUnitOfWork unitOfWork, IRedisCaching caching)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _participantRepository = participantRepository;
            _eventRepository = eventRepository;
            _synchronizationProvider = synchronizationProvider;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(CreatePayment request, CancellationToken cancellationToken)
        {
            var @lock = _synchronizationProvider.CreateLock($"Event_{(Guid)request.UserId}");
            await using (var handle = await @lock.TryAcquireAsync())
            {
                if (handle == null)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.Conflict,
                        Message = MessageCommon.lockAcquired
                    };
                }


                Random rnd = new Random();
                string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + rnd.Next(1000000);
                string cacheKey = $"payment_{app_trans_id}";
                var appUser = "user123";

                //call api to create transaction
                var result = await _zaloPayService.CreateOrderAsync(request.Amount, appUser, request.Description!, app_trans_id, request.redirectUrl!);
                var returnCode = (long)result["return_code"];
                var orderUrl = (string)result["order_url"];

                var existUser = await _userRepository.GetById(request.UserId);
                if (existUser == null)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        Message = MessageUser.UserNotFound
                    };
                }

                //check if type is not subscription then check event existed or not
                if (request.SubscriptionType != (int)PaymentType.SUBSCRIPTION)
                {
                    var existEvent = await _eventRepository.GetById(request.EventId!);
                    if (existEvent == null)
                    {
                        return new APIResponse
                        {
                            StatusResponse = HttpStatusCode.NotFound,
                            Message = MessageEvent.EventIdNotExist
                        };
                    }

                    var existTransaction = await _transactionRepository.GetExistProcessingTransaction(request.UserId, (Guid)request.EventId!);
                    if (existTransaction != null)
                    {
                        return new APIResponse
                        {
                            StatusResponse = HttpStatusCode.BadRequest,
                            Message = MessagePayment.ALREADY_HAVE_PROCESSING_TICKET_ORDER
                        };
                    }

                    if (request.SubscriptionType == (int)PaymentType.TICKET)
                    {
                        //check if event is still have slot and using lock
                        var @localLock = _synchronizationProvider.CreateLock($"Event_{(Guid)request.EventId}");
                        await using (var handleLocal = await @localLock.TryAcquireAsync())
                        {
                            if (handleLocal == null)
                            {
                                return new APIResponse
                                {
                                    StatusResponse = HttpStatusCode.Conflict,
                                    Message = MessageCommon.lockAcquired
                                };
                            }

                            // Kiểm tra nếu user đã thanh toán vé này rồi
                            var existPayment = await _transactionRepository.GetAlreadyPaid(request.UserId, (Guid)request.EventId!);
                            if (existPayment != null)
                            {
                                return new APIResponse
                                {
                                    StatusResponse = HttpStatusCode.BadRequest,
                                    Message = MessagePayment.ALREADY_PAID_THIS_TICKET
                                };
                            }

                            // Kiểm tra sức chứa của sự kiện
                            var capacityCheck = await _participantRepository.IsReachedCapacity((Guid)request.EventId);
                            if (capacityCheck)
                            {
                                return new APIResponse
                                {
                                    StatusResponse = HttpStatusCode.BadRequest,
                                    Message = MessageParticipant.ParticipantCapacityLimitReached
                                };
                            }
                        }
                    }


                }



                //save transaction to db and also save in redis if create transaction successfully in zalopay
                if (returnCode == 1)
                {
                    var newTrans = new Transaction
                    {
                        Apptransid = app_trans_id,
                        Amount = request.Amount,
                        Description = request.Description,
                        Timestamp = Utils.GetTimeStamp(),
                        Status = (int)TransactionStatus.PROCESSING,
                        CreatedAt = DateTime.UtcNow,
                        UserId = request.UserId,
                        EventId = (request.SubscriptionType != (int)PaymentType.SUBSCRIPTION) ? request.EventId : null,
                        SubscriptionType = request.SubscriptionType,
                        OrderUrl = orderUrl,
                    };

                    await _transactionRepository.Add(newTrans);
                    await _unitOfWork.SaveChangesAsync();

                    //caching transaction
                    var hashEntries = new HashEntry[]
                            {
                            new HashEntry("transactionId",$"{newTrans.Apptransid}"),
                            new HashEntry("timestamp", $"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()}")
                            };
                    await _caching.HashSetAsync(cacheKey, hashEntries, 15);
                }


                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = result
                };
            }
        }
    }
}

