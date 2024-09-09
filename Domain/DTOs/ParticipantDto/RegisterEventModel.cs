using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ParticipantDto
{
    public class RegisterEventModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        [Range(1, 4)]
        public int RoleEventId { get; set; }

        public RegisterEventModel()
        {
        }
        public RegisterEventModel(Guid userId, Guid eventId, int roleId)
        {
            EventId = eventId;
            UserId = userId;
            RoleEventId = roleId;
        }
    }
}
