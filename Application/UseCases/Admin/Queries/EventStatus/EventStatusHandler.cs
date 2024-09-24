using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using MediatR.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.EventStatus;

public class EventStatusHandler : IRequestHandler<EventStatusQuery, APIResponse>
{
    private readonly IEventRepository _eventRepository;

    public EventStatusHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<APIResponse> Handle(EventStatusQuery request, CancellationToken cancellationToken)
    {
        var response = await _eventRepository.CountByStatus();
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = response
        };
    }
}
