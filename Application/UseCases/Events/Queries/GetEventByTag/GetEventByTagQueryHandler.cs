using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventByTag
{
    public class GetEventByTagQueryHandler : IRequestHandler<GetEventByTagQuery, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetEventByTagQueryHandler(IEventRepository eventRepo, IMapper mapper, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventByTagQuery request, CancellationToken cancellationToken)
        {
            // Generate the cache key based on the request
            var cacheKey = GenerateCacheKey(request);

            // Check if data exists in the cache
            var cachedData = await _redisCaching.GetAsync<PagedList<EventResponseDto>>(cacheKey);

            if (cachedData != null)
            {
                // Return the cached data if it exists
                return cachedData;
            }


            var result = await _eventRepo.GetEventsByListTags(request.TagIds, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);

            //await _redisCaching.SetAsync(cacheKey, pages, TimeSpan.FromMinutes(10).TotalMinutes);
            await _redisCaching.SetAsync("test_key", "test_value", 600); // 600 seconds
            var value = await _redisCaching.GetAsync<string>("test_key");
            Console.WriteLine(value);  // Should print "test_value"

            return pages;
        }

        private string GenerateCacheKey(GetEventByTagQuery request)
        {
            var tagIds = string.Join("_", request.TagIds);
            return $"events_by_tags_{tagIds}_page_{request.PageNo}_size_{request.ElementEachPage}";
        }

    }
}
