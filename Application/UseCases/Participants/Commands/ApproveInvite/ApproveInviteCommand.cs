using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Commands.ApproveInvite
{
    public class ApproveInviteCommand: IRequest<APIResponse>
    {
        public Guid EventId { get; set; }
        public Guid UserId {  set; get; }
        public string status { get; set; }
        public ApproveInviteCommand(Guid eventId, Guid userId, string status)
        {
            EventId = eventId;
            UserId = userId;
            this.status = status;
        }
    }
}
