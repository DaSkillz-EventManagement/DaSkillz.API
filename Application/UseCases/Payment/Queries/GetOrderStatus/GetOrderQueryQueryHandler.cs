using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper.Crypto;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaloPay.Helper;

namespace Application.UseCases.Payment.Queries.GetOrderStatus
{
    public class GetOrderQueryQueryHandler : IRequestHandler<GetOrderStatusQuery, APIResponse>
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderQueryQueryHandler(IZaloPayService zaloPayService, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _zaloPayService = zaloPayService;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _zaloPayService.QueryOrderStatus(request.appTransId!);
            var exist = await _transactionRepository.GetById(request.appTransId!);
            if (exist != null)
            {
                exist.Zptransid = result["zp_trans_id"].ToString();
                if (!(bool)result["is_processing"])
                {
                    exist.Status = (int)TransactionStatus.SUCCESS;
                }
                await _unitOfWork.SaveChangesAsync();
                
            }
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = (string)result["return_message"],
                Data = result,
            };
        }
    }
}
