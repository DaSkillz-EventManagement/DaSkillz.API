using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ParticipantDto
{
    public class ParticipantTicket
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
