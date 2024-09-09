using AutoMapper;
using Domain.DTOs.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Command.GetEventByUserRole
{
    public class GetEventByUserRoleCommandHandler : IRequestHandler<GetEventByUserRoleCommand, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetEventByUserRoleCommandHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventByUserRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.getEventByUserRole(request.EventRole, request.UserId, request.PageNo, request.ElementEachPage);
           
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
