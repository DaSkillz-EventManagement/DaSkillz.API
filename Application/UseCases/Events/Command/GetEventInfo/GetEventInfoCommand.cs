using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetEvent
{
    public class GetEventInfoCommand : IRequest<APIResponse>
    {
        public Guid Id { get; set; }

        public GetEventInfoCommand(Guid id)
        {
            Id = id;
        }
    }
}
