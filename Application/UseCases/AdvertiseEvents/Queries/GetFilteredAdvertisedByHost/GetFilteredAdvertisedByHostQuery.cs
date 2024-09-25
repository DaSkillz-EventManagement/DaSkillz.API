using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Queries.GetFilteredAdveetisedByHost
{
    public class GetFilteredAdvertisedByHostQuery : IRequest<APIResponse>
    { 
        public Guid UserId { get; set; }

        public string Status {  get; set; }

        public GetFilteredAdvertisedByHostQuery(Guid userId, string status)
        {
            UserId = userId;
            Status = status;
        }
    }
}
