using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.RegisterEventCommand;

public class RegisterEventCommand: IRequest<APIResponse>
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public Guid? TransactionId { get; set; }
    public RegisterEventCommand(Guid userId, Guid? transactionId, Guid eventId)
    {
        UserId = userId;
        TransactionId = transactionId;
        EventId = eventId;
    }
}
