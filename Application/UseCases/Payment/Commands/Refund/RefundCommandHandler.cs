using Application.Abstractions.Payment.ZaloPay;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Commands.Refund
{
    public class RefundCommandHandler : IRequestHandler<RefundCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IZaloPayService _zaloPayService;
        private readonly IRefundTransactionRepository _refundTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;

        public RefundCommandHandler(IUnitOfWork unitOfWork, IZaloPayService zaloPayService, IRefundTransactionRepository refundTransactionRepository, ITransactionRepository transactionRepository)
        {
            _unitOfWork = unitOfWork;
            _zaloPayService = zaloPayService;
            _refundTransactionRepository = refundTransactionRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(RefundCommand request, CancellationToken cancellationToken)
        {
            var existTransaction = await _transactionRepository.GetById(request.AppTransId!);
            if (existTransaction == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = null,
                };
            }

            if (existTransaction.Zptransid == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = null,
                };
            }

            var apiResponse = await _zaloPayService.Refund(existTransaction.Zptransid, existTransaction.Amount, request.description);

            var returnCode = Convert.ToInt32(apiResponse["return_code"]);
            var returnMessage = apiResponse["return_message"].ToString();
            var refundId = (long)apiResponse["refund_id"];
            var refundAmount = existTransaction.Amount;

            var refundTransaction = new RefundTransaction
            {
                refundId = refundId,
                returnCode = returnCode,
                returnMessage = returnMessage,
                refundAmount = long.Parse(refundAmount),
                refundAt = DateTime.UtcNow,
                Apptransid = existTransaction.Apptransid,
                Transaction = existTransaction
            };

            await _refundTransactionRepository.Add(refundTransaction);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Create refund transaction successfully",
                Data = apiResponse,
            };
        }
    }
}
