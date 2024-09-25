using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Sponsor.Commands.DeleteSponsorRequest;

public class DeleteSponsorRequestCommand : IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public DeleteSponsorRequestCommand(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
    }
}
