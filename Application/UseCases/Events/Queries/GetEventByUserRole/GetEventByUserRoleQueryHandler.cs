using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventByUserRole
{
    public class GetEventByUserRoleQueryHandler : IRequestHandler<GetEventByUserRoleQuery, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetEventByUserRoleQueryHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<PagedList<EventResponseDto>> Handle(GetEventByUserRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.getEventByUserRole(request.EventRole, request.UserId, request.PageNo, request.ElementEachPage);

            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
