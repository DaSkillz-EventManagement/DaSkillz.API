using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetAllTransaction
{
    public class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetAllTransactionQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
        {
            var result = await _transactionRepository.GetAll();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result,
            };
        }
    }
}
