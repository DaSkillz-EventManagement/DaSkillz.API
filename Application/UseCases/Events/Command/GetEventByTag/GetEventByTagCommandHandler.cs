using AutoMapper;
using Domain.DTOs.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Command.GetEventByTag
{
    public class GetEventByTagCommandHandler : IRequestHandler<GetEventByTagCommand, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetEventByTagCommandHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventByTagCommand request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.GetEventsByListTags(request.TagIds, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
