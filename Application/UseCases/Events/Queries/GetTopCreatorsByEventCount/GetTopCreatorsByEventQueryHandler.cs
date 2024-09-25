using Application.Abstractions.Caching;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetTopCreatorsByEventCount
{
    public class GetTopCreatorsByEventQueryHandler : IRequestHandler<GetTopCreatorsByEventQuery, List<EventCreatorLeaderBoardDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IRedisCaching _redisCaching;

        public GetTopCreatorsByEventQueryHandler(IEventRepository eventRepo, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventCreatorLeaderBoardDto>> Handle(GetTopCreatorsByEventQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetTopCreatorsByEvent";
            var cachedDataString = await _redisCaching.GetAsync<List<EventCreatorLeaderBoardDto>>(cacheKey);
            if (cachedDataString != null)
            {
                return cachedDataString;
            }
            var result = await _eventRepo.GetTop10CreatorsByEventCount();
            await _redisCaching.SetAsync(cacheKey, result, 10);
            return result;
        }
    }
}
