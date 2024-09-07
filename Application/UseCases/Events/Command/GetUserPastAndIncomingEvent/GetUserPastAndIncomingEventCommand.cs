using Domain.DTOs.Events;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetUserPastAndIncomingEvent
{
    public class GetUserPastAndIncomingEventCommand : IRequest<Dictionary<string, List<EventPreviewDto>>>
    {
        public Guid UserId {  get; set; }

        public GetUserPastAndIncomingEventCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
