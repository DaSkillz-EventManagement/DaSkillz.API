using Domain.DTOs.Events.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Sponsors;

public class SponsorEventDetailDto
{
    public Guid? EventId { get; set; }
    public Guid? UserId { get; set; }
    public string? Status { get; set; }
    public bool? IsSponsored { get; set; }
    public string? SponsorType { get; set; }
    public decimal? Amount { get; set; }
    public string? Message { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public EventResponseDto eventResponseDto { get; set; } = new EventResponseDto();
}
