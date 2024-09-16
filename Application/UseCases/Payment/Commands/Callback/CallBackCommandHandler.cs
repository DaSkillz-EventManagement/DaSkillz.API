using Application.Abstractions.Payment.ZaloPay;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Commands.Callback
{
    public class CallBackCommandHandler : IRequestHandler<CallbackCommand, object>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IZaloPayService _zaloPayService;

        public CallBackCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IZaloPayService zaloPayService)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _zaloPayService = zaloPayService;
        }

        public async Task<object> Handle(CallbackCommand request, CancellationToken cancellationToken)
        {
            try
            {


                var dataStr = Convert.ToString(request.data);
                var reqMac = Convert.ToString(request.mac);

                var isValid = _zaloPayService.ValidateMac(dataStr, reqMac);

                if (!isValid)
                {
                    return new
                    {
                        ReturnCode = -1,
                        ReturnMessage = "mac not equal"
                    };
            }

                // Deserialize data using the infrastructure service
                var dataJson = _zaloPayService.DeserializeData(dataStr);
                var appid = Convert.ToString(dataJson["app_trans_id"]);

                // Fetch and update the order in the application layer
                var exist = await _transactionRepository.GetById(appid);

                if (exist != null)
                {
                    exist.Status = (int)TransactionStatus.SUCCESS;
                    await _transactionRepository.Update(exist);
                    await _unitOfWork.SaveChangesAsync();
                }


                return new
                {
                    ReturnCode = 1,
                    ReturnMessage = "success"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    ReturnCode = 0,
                    ReturnMessage = ex.Message,
                };
            }
        }
    }
}
