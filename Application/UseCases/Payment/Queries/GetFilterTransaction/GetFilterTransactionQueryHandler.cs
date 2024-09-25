using Application.UseCases.Payment.Queries.GetTransactionByEvent;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetFilterTransaction
{
    public class GetFilterTransactionQueryHandler : IRequestHandler<GetFilterTransactionQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetFilterTransactionQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetFilterTransactionQuery request, CancellationToken cancellationToken)
        {
            // Gọi repository để lọc dữ liệu dựa trên eventId, userId, status, subscriptionType
            var transactions = await _transactionRepository.FilterTransactionsAsync(
                request.eventId,
                request.userId,
                request.status,
                request.SubscriptionType);

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = "Success",
                Data = transactions
            };
        }
    }
}
