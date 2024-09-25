using Domain.DTOs.ParticipantDto;
using MediatR;

namespace Application.UseCases.Participants.Queries.GetCurrentUser
{
    public class GetCurrentUserQueries : IRequest<ParticipantEventDto>
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public GetCurrentUserQueries(Guid userId, Guid eventId)
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}
