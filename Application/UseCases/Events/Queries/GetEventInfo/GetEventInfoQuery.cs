using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetEventInfo
{
    public class GetEventInfoQuery : IRequest<APIResponse>
    {
        public Guid Id { get; set; }

        public GetEventInfoQuery(Guid id)
        {
            Id = id;
        }
    }
}
