using Domain.DTOs;
using Domain.DTOs.Events;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.CreateEvent
{
    public class CreateEventCommand : IRequest<APIResponse>
    {
       public EventRequestDto EventRequestDto { get; set; }

        public CreateEventCommand(EventRequestDto eventRequestDto)
        {
            EventRequestDto = eventRequestDto;
        }
    }
}
