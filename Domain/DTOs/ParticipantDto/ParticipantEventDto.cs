namespace Domain.DTOs.ParticipantDto;

public class ParticipantEventDto
{
    public Guid UserId { get; set; }
    public int? RoleEventId { get; set; }
    public DateTime? CheckedIn { get; set; }
    public bool? IsCheckedMail { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
}
