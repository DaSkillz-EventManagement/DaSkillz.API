using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Logo
    {
        public Logo()
        {
            Events = new HashSet<Event>();
        }

        public int LogoId { get; set; }
        public string? SponsorBrand { get; set; }
        public string? LogoUrl { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
