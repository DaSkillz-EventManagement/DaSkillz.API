using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.EventStatus;

public class EventStatusQuery: IRequest<APIResponse>
{
    public EventStatusQuery() { }
}
