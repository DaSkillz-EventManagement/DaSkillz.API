using Application.Abstractions.Caching;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetTopLocationByEventCount
{
    public class GetTopLocationByEventQueryHandler : IRequestHandler<GetTopLocationByEventQuery, List<EventLocationLeaderBoardDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IRedisCaching _redisCaching;

        public GetTopLocationByEventQueryHandler(IEventRepository eventRepo, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventLocationLeaderBoardDto>> Handle(GetTopLocationByEventQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"getEv_TopLocations";
            var cachedDataString = await _redisCaching.GetAsync<List<EventLocationLeaderBoardDto>>(cacheKey);
            if (cachedDataString != null)
            {
                return cachedDataString;
            }

            var result =  await _eventRepo.GetTop10LocationByEventCount();
            await _redisCaching.SetAsync(cacheKey, result, 10);
            return result;
        }
    }
}
