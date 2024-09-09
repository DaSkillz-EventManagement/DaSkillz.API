using Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Events
{
    public class EventPreviewDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string? Status { get; set; }
        public long StartDate { get; set; } 
        public CreatedByUserDto? Host { get; set; } = new CreatedByUserDto();
        public string? Image { get; set; }
        public string? Location { get; set; }
    }
}
