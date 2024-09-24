namespace Domain.DTOs.Events.ResponseDto;

public class ParticipantInfo
{
    public Guid UserId { get; set; }
    public int? RoleEventId { get; set; }
    public string? Status { get; set; }
}
