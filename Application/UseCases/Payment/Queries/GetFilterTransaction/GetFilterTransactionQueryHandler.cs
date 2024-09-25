using Application.ResponseMessage;
using Application.UseCases.Payment.Queries.GetTransactionByEvent;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Application.UseCases.Payment.Queries.GetFilterTransaction
{
    public class GetFilterTransactionQueryHandler : IRequestHandler<GetFilterTransactionQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetFilterTransactionQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFilterTransactionQuery request, CancellationToken cancellationToken)
        {
            // Gọi repository để lọc dữ liệu dựa trên eventId, userId, status, subscriptionType
            var transactions = await _transactionRepository.FilterTransactionsAsync(
                request.eventId,
                request.userId,
            request.status,
            request.SubscriptionType);

            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }
    }
}
