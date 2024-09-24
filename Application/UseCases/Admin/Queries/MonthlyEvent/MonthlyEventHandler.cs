using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.MonthlyEvent;

public class MonthlyEventHandler : IRequestHandler<MonthlyEventQuery, APIResponse>
{
    private readonly IEventRepository _eventRepository;

    public MonthlyEventHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<APIResponse> Handle(MonthlyEventQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.EventsPerMonth(request.StartDate, request.EndDate);
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        };
    }
}
