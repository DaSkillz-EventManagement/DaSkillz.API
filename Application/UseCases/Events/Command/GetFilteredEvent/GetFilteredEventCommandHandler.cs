using Application.UseCases.Events.Command.GetEventByUserRole;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetFilteredEvent
{

    public class GetFilteredEventCommandHandler : IRequestHandler<GetFilteredEventCommand, PagedList<EventResponseDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;


        public async Task<PagedList<EventResponseDto>> Handle(GetFilteredEventCommand request, CancellationToken cancellationToken)
        {
            var result = await _eventRepo.GetFilteredEvent(request.Filter, request.PageNo, request.ElementEachPage);
            var response = _mapper.Map<PagedList<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.TotalItems, request.PageNo, request.ElementEachPage);
            return pages;
        }
    }
}
