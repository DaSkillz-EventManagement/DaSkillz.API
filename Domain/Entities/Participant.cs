using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Participant
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int? RoleEventId { get; set; }
        public DateTime? CheckedIn { get; set; }
        public bool? IsCheckedMail { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual RoleEvent? RoleEvent { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
