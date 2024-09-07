using Application.UseCases.Events.Command.GetEventByUserRole;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserCommandHandler : IRequestHandler<GetEventParticipatedByUserCommand, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetEventParticipatedByUserCommandHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }



        public async Task<PagedList<EventResponseDto>> Handle(GetEventParticipatedByUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.GetUserParticipatedEvents(request.Filter, request.UserId, request.PageNo, request.ElementEachPage);
            //List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
