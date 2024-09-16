using Domain.Enum.Participant;
using Event_Management.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ParticipantDto;
public class FilterParticipantDto
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    [Range(1, int.MaxValue)]
    public int EachPage { get; set; } = 10;
    [Required]
    public Guid EventId { get; set; }
    [Range(1, 4)]
    public int? RoleEventId { get; set; }
    public ParticipantStatus Status { get; set; } = ParticipantStatus.Confirmed;
    public DateTime? CheckedIn { get; set; }
    public bool? IsCheckedMail { get; set; }
    public DateTime? CreatedAt { get; set; }
    public ParticipantSortBy SortBy { get; set; } = ParticipantSortBy.CreatedAt;
}
