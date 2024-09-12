using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Queries.GetCurrentUser
{
    public class GetCurrentUserQueries: IRequest<ParticipantEventDto>
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
