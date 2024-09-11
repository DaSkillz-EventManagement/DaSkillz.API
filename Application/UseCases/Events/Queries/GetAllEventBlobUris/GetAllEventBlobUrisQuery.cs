using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetAllEventBlobUris
{
    public class GetAllEventBlobUrisQuery : IRequest<Dictionary<string, List<string>>>
    {

        public Guid EventId { get; set; }

        public GetAllEventBlobUrisQuery(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
