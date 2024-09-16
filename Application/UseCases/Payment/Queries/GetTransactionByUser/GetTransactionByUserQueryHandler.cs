using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetTransactionByUser
{
    public class GetTransactionByUserQueryHandler : IRequestHandler<GetTransactionByUserQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByUserQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetTransactionByUserQuery request, CancellationToken cancellationToken)
        {
            var exist = await _transactionRepository.getTransactionByUserIdAsync(request.userId);
            bool checkExist = exist != null;
            return new APIResponse
            {
                StatusResponse = checkExist ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Message = checkExist ? MessageCommon.GetSuccesfully : MessageCommon.GetFailed,
                Data = checkExist ? exist : null,
            };
        }
    }
}
