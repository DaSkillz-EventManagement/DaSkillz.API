using Domain.DTOs.AdvertisedEvents;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent
{
    public class UseAdvertisedEventQuery : IRequest<APIResponse>
    {
        public AdvertisedEventDto AdvertisedEventDto { get; set; }
        public Guid UserId {  get; set; }

        public UseAdvertisedEventQuery(AdvertisedEventDto advertisedEventDto)
        {
            AdvertisedEventDto = advertisedEventDto;
        }
    }

}
