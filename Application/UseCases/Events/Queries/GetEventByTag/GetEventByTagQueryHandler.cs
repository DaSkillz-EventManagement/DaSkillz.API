using AutoMapper;
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

        public GetEventByTagQueryHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventByTagQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.GetEventsByListTags(request.TagIds, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
