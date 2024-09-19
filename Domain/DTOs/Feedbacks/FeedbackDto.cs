using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Feedbacks
{
    public class FeedbackDto
    {
        public Guid EventId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
    }
}
