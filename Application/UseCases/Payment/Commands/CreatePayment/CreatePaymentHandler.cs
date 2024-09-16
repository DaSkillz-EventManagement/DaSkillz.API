using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePayment, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentHandler(ITransactionRepository transactionRepository, IZaloPayService zaloPayService, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
        }


        public async Task<APIResponse> Handle(CreatePayment request, CancellationToken cancellationToken)
        {
            var appUser = "user123";
            string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + new Random().Next(100000);

            var result = await _zaloPayService.CreateOrderAsync(request.Amount, appUser, request.Description!, app_trans_id);
            var returnCode = (long)result["return_code"];

            if (returnCode == 1)
            {
                await _transactionRepository.Add(new Transaction
                {
                    Apptransid = app_trans_id,
                    Amount = request.Amount,
                    Timestamp = Utils.GetTimeStamp(),
                    Description = request.Description,
                    Status = 0,
                    CreatedAt = DateTime.UtcNow,
                    //UserId = request.UserId,
                    //EventId = request.EventId,
                });

                await _unitOfWork.SaveChangesAsync();
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
