using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Events
{
    public class EventRequestDto
    {
        [Required(ErrorMessage = "EventName is required!")]
        [MinLength(3, ErrorMessage = "Event name must be between 3 and 250 characters!")]
        [MaxLength(250, ErrorMessage = "Event name must be between 3 and 250 characters!")]
        public string EventName { get; set; } = null!;

        [Required(ErrorMessage = "Event Description is required!")]
        [MinLength(3, ErrorMessage = "Event Description must be between 3 and 5000 characters!")]
        [MaxLength(5000, ErrorMessage = "Event Description must be between 3 and 5000 characters!")]
        public string? Description { get; set; } = null!;

        public List<int> TagId { get; set; } = new List<int>();

        [Required(ErrorMessage = "StartDate is required")]
        public long StartDate { get; set; } = DateTime.Now.AddDays(1).Ticks;
        [Required(ErrorMessage = "EndDate is required")]
        public long EndDate { get; set; } = DateTime.Now.AddDays(2).Ticks;
        public string? Theme { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        public EventLocation? Location { get; set; }

        [Required(ErrorMessage = "Capacity is required!")]
        public int? Capacity { get; set; } = 30;

        public bool? Approval { get; set; } = false;

        [Range(0, 5000000, ErrorMessage = "Maximum ticket price is 5 000 000")]
        public decimal? Ticket { get; set; } = 0;
    }
}
