using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.ProcessTicketParticipant
{
    public class ProcessTicketParticipantCommand : IRequest<APIResponse>
    {
        public ParticipantTicket ParticipantTicket { get; set; }
        public Guid UserId { get; set; }
        public ProcessTicketParticipantCommand(ParticipantTicket participantTicket, Guid userId)
        {
            ParticipantTicket = participantTicket;
            UserId = userId;
        }
    }
}
