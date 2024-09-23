using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ParticipantDto;

public class TicketModel
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string? OrgainzerName { get; set; }
    public int RoleEventId { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public string? LogoEvent { get; set; }
    public string? EventName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string? LocationAddress { get; set; }
    public string? LocationUrl { get; set; }
    public string? Time { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? TypeButton { get; set; } = string.Empty;
}
