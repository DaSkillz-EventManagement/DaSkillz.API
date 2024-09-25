using Domain.DTOs.ParticipantDto;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Participants.Queries.GetParticipantOnEvent
{
    public class GetParticipantOnEventQueries : IRequest<PagedList<ParticipantEventDto>>
    {
        public FilterParticipantDto FilterParticipant { get; set; }
        public Guid UserId { get; set; }
        public GetParticipantOnEventQueries(FilterParticipantDto filterParticipant, Guid userId)
        {
            FilterParticipant = filterParticipant;
            UserId = userId;
        }
    }
}
