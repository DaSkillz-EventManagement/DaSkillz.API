using Domain.DTOs.ParticipantDto;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Queries.GetParticipantOnEvent
{
    public class GetParticipantOnEventQueries: IRequest<PagedList<ParticipantEventDto>>
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
