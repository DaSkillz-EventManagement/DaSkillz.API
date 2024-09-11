using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserQueryHandler : IRequestHandler<GetEventParticipatedByUserQuery, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetEventParticipatedByUserQueryHandler(IEventRepository eventRepo, IMapper mapper, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventParticipatedByUserQuery request, CancellationToken cancellationToken)
        {
            string RedisDbkey = $" GetEventParticipatedByUser_{request.UserId}";
            var existingGetEventParticipatedByUser = await _redisCaching.GetAsync<PagedList<EventResponseDto>>(RedisDbkey);
            if(existingGetEventParticipatedByUser != null)
            {
                return existingGetEventParticipatedByUser;
            }

            var result = await _eventRepo.GetUserParticipatedEvents(request.Filter, request.UserId, request.PageNo, request.ElementEachPage);
            //List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            await _redisCaching.SetAsync(RedisDbkey, pages, 5);
            return pages;
        }
    }
}
