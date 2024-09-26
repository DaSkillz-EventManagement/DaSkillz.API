
using Application.Abstractions.Caching;
using Application.Helper;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.UseCases.Events.Queries.GetFilteredEvent
{

    public class GetFilteredEventQueryHandler : IRequestHandler<GetFilteredEventQuery, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetFilteredEventQueryHandler(IEventRepository eventRepo, IMapper mapper, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetFilteredEventQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = EventHelper.GenerateCacheKeyFilteredEvent(request.Filter, request.PageNo, request.ElementEachPage);

            //var cachedDataString = await _redisCaching.GetAsync<string>(cacheKey);
            //if (cachedDataString != null)
            //{
            //    // Deserialize into a helper object first (using List<T> for Items)
            //    var jsonObject = JsonConvert.DeserializeObject<JObject>(cachedDataString);

            //    // Manually extract properties from the cached data
            //    var items = jsonObject["Items"].ToObject<List<EventResponseDto>>();
            //    var totalItems = jsonObject["TotalItems"].Value<int>();
            //    var currentPage = jsonObject["CurrentPage"].Value<int>();
            //    var eachPage = jsonObject["EachPage"].Value<int>();

            //    // Create the PagedList<EventResponseDto> manually
            //    return new PagedList<EventResponseDto>(
            //        items: items,
            //        totalItems: totalItems,
            //        currentPage: currentPage,
            //        eachPage: eachPage
            //    );
            //}


            var result = await _eventRepo.GetFilteredEvent(request.Filter, request.PageNo, request.ElementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>(); //_mapper.Map<List<EventResponseDto>>(result);
            response = result.Select(_eventRepo.ToResponseDto).ToList();
            var pages = new PagedList<EventResponseDto>
                (response, result.TotalItems, request.PageNo, request.ElementEachPage);

            //// Serialize and cache the PagedList in Redis
            //var serializedPagedList = JsonConvert.SerializeObject(new
            //{
            //    Items = pages.Items,
            //    TotalItems = pages.TotalItems,
            //    CurrentPage = pages.CurrentPage,
            //    EachPage = pages.EachPage
            //});

            //await _redisCaching.SetAsync(cacheKey, serializedPagedList, TimeSpan.FromMinutes(10).TotalMinutes);

            return pages;
        }
    }
}
