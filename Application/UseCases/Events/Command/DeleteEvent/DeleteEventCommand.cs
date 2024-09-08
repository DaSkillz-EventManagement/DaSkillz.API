using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.DeleteEvent
{
    public class DeleteEventCommand : IRequest<bool>
    {
        public Guid EventId { get; set; }
        public Guid UserId {  get; set; }

        public DeleteEventCommand(Guid eventId)
        {
            EventId = eventId;
           
        }
    }
}
