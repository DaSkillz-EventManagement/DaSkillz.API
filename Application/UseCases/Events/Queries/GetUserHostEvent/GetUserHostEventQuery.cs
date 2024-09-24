using Domain.DTOs.Events;
using MediatR;

namespace Application.UseCases.Events.Queries.GetUserHostEvent
{
    public class GetUserHostEventQuery : IRequest<List<EventPreviewDto>>
    {
        public Guid UserId { get; set; }

        public GetUserHostEventQuery()
        {
        }

        public GetUserHostEventQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
