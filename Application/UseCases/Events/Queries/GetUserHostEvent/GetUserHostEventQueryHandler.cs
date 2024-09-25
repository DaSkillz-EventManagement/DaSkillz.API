using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetUserHostEvent
{
    public class GetUserHostEventQueryHandler : IRequestHandler<GetUserHostEventQuery, List<EventPreviewDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetUserHostEventQueryHandler(IEventRepository eventRepo, IMapper mapper, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventPreviewDto>> Handle(GetUserHostEventQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetUserHostEvent_{request.UserId}";
            var cachedDataString = await _redisCaching.GetAsync<List<EventPreviewDto>>(cacheKey);
            if (cachedDataString != null)
            {
                return cachedDataString;
            }

            var result = await _eventRepo.GetUserHostEvent(request.UserId);
            var eventPreview = result.Select(_eventRepo.ToEventPreview);

            await _redisCaching.SetAsync(cacheKey, eventPreview, 10);
            return eventPreview.ToList();
        }


    }
}
