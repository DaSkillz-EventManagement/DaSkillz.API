using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedEventByCreatedDay
{
    public class GetAdvertisedEventQuery : IRequest<APIResponse>
    {
    }
}
