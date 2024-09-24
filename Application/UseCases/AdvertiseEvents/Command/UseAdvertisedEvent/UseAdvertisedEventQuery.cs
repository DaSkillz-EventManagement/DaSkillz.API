using Domain.DTOs.AdvertisedEvents;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent
{
    public class UseAdvertisedEventQuery : IRequest<APIResponse>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public UseAdvertisedEventQuery(Guid eventId, Guid userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    }

}
