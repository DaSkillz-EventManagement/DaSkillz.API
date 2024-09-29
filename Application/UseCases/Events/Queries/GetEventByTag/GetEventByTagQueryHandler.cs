
using Application.Abstractions.Caching;
using Application.Helper;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            var cacheKey = EventHelper.GenerateCacheKeyByTag(request);


            var cachedDataString = await _redisCaching.GetAsync<string>(cacheKey);
            if (cachedDataString != null)
            {

                var jsonObject = JsonConvert.DeserializeObject<JObject>(cachedDataString);


                var items = jsonObject["Items"].ToObject<List<EventResponseDto>>();
                var totalItems = jsonObject["TotalItems"].Value<int>();
                var currentPage = jsonObject["CurrentPage"].Value<int>();
                var eachPage = jsonObject["EachPage"].Value<int>();


                return new PagedList<EventResponseDto>(
                    items: items,
                    totalItems: totalItems,
                    currentPage: currentPage,
                    eachPage: eachPage
                );
            }


            var result = await _eventRepo.GetEventsByListTags(request.TagIds, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            var pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);


            var serializedPagedList = JsonConvert.SerializeObject(new
            {
                Items = pages.Items,
                TotalItems = pages.TotalItems,
                CurrentPage = pages.CurrentPage,
                EachPage = pages.EachPage
            });

            await _redisCaching.SetAsync(cacheKey, serializedPagedList, 10);

            return pages;
        }
    }
}
