using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sponsor.Commands.DeleteSponsorRequest;

public class DeleteSponsorRequestCommand: IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public DeleteSponsorRequestCommand(Guid eventId, Guid userId) {
        EventId = eventId;
        UserId = userId;
    }
}
