using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedInfoByEvent
{
    public class GetAdvertisedInfoByEventQuery : IRequest<APIResponse>
    {
        public Guid EventId {  get; set; }
        public Guid UserId { get; set; }

        public GetAdvertisedInfoByEventQuery(Guid eventId, Guid userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}
