using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

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
            string RedisDbkey = $"GetFiltered";

            var existingGetFilteredEvent = await _redisCaching.GetAsync<PagedList<EventResponseDto>>(RedisDbkey);
            if(existingGetFilteredEvent != null)
            {
                return existingGetFilteredEvent;
            }


            var result = await _eventRepo.GetFilteredEvent(request.Filter, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            await _redisCaching.SetAsync(RedisDbkey, pages, 5);
            return pages;
        }
    }
}
