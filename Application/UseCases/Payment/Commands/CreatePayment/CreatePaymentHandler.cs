using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using StackExchange.Redis;
using System.Net;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePayment, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCaching _caching;

        public CreatePaymentHandler(ITransactionRepository transactionRepository, IUserRepository userRepository, IEventRepository eventRepository, IZaloPayService zaloPayService, IUnitOfWork unitOfWork, IRedisCaching caching)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(CreatePayment request, CancellationToken cancellationToken)
        {
            string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + new Random().Next(100000);
            string cacheKey = $"payment_{app_trans_id}";
            var appUser = "user123";

            //call api to create transaction
            var result = await _zaloPayService.CreateOrderAsync(request.Amount, appUser, request.Description!, app_trans_id);
            var returnCode = (long)result["return_code"];

            var existUser = await _userRepository.GetById(request.UserId);
            if (existUser == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageUser.UserNotFound
                };
            }

            if (!request.isSubscription)
            {
                var existEvent = await _eventRepository.GetById(request.EventId);
                if (existEvent == null)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        Message = MessageEvent.EventIdNotExist
                    };
                }
            }
            

            var newTrans = new Transaction
            {
                Apptransid = app_trans_id,
                Amount = request.Amount,
                Description = request.Description,
                Timestamp = Utils.GetTimeStamp(),
                Status = (int)TransactionStatus.PROCESSING,
                CreatedAt = DateTime.UtcNow,
                UserId = request.UserId,
                EventId = (!request.isSubscription) ? request.EventId : null,
                IsSubscription = request.isSubscription
            };

            //save transaction to db and also save in redis if create transaction successfully in zalopay
            if (returnCode == 1)
            {
                await _transactionRepository.Add(newTrans);
                await _unitOfWork.SaveChangesAsync();

                //caching transaction
                var hashEntries = new HashEntry[]
                        {
                            new HashEntry("transactionId",$"{newTrans.Apptransid}"),
                            new HashEntry("timestamp", $"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()}")
                        };
                await _caching.HashSetAsync(cacheKey, hashEntries, 16);
            }

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "test",
                Data = result
            };
        }
    }
}
