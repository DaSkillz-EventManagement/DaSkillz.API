namespace Domain.DTOs.ParticipantDto
{
    public class ParticipantDto
    {
        public Guid UserId { get; set; }
        public int? RoleEventId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Status { get; set; } = null!;
        public DateTime? CheckedIn { get; set; }
    }
}
