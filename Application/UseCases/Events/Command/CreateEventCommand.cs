using Domain.DTOs;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command
{
    public class CreateEventCommand : IRequest<APIResponse>
    {
        public string EventName { get; set; } = null!;


        public string? Description { get; set; } = null!;

        public List<int> TagId { get; set; } = new List<int>();

        public long StartDate { get; set; } = DateTime.Now.AddDays(1).Ticks;

        public long EndDate { get; set; } = DateTime.Now.AddDays(2).Ticks;
        public string? Theme { get; set; }
        public string? Image { get; set; }

        public EventLocation? Location { get; set; }

        public int? Capacity { get; set; } = 30;

        public bool? Approval { get; set; } = false;


        public decimal? Ticket { get; set; } = 0;

        public CreateEventCommand(string eventName, string? description, List<int> tagId, long startDate, long endDate, string? theme, string? image, EventLocation? location, int? capacity, bool? approval, decimal? ticket)
        {
            EventName = eventName;
            Description = description;
            TagId = tagId;
            StartDate = startDate;
            EndDate = endDate;
            Theme = theme;
            Image = image;
            Location = location;
            Capacity = capacity;
            Approval = approval;
            Ticket = ticket;
        }
    }
}
