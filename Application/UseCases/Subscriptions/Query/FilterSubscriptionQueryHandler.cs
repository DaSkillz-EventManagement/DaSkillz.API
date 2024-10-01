using Application.ResponseMessage;
using Application.UseCases.Payment.Queries.GetFilterTransaction;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.Subscription;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Subscriptions.Query
{
    public class FilterSubscriptionQueryHandler : IRequestHandler<FIilterSubscriptionQuery, APIResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public FilterSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(FIilterSubscriptionQuery request, CancellationToken cancellationToken)
        {
            // Gọi repository để lọc dữ liệu dựa trên eventId, userId, status, subscriptionType
            var result = await _subscriptionRepository.FilterSubscriptionAsync(
                request.userId,
            request.isActive);

            var mapResult = _mapper.Map<IEnumerable<SubscriptionResponseDto>>(result);
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = mapResult
            };
        }
    }
}
