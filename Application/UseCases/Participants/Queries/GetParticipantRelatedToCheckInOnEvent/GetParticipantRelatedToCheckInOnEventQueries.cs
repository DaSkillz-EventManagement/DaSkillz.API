using Domain.DTOs.ParticipantDto;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Participants.Queries.GetParticipantRelatedToCheckInOnEvent
{
    public class GetParticipantRelatedToCheckInOnEventQueries : IRequest<PagedList<ParticipantDto>>
    {
        public Guid EventId { get; set; }
        public int page { get; set; }
        public int eachPage { get; set; }
        public Guid UserId { get; set; }
        public GetParticipantRelatedToCheckInOnEventQueries(Guid eventId, int page, int eachPage, Guid userId)
        {
            EventId = eventId;
            this.page = page;
            this.eachPage = eachPage;
            UserId = userId;
        }
    }
}
