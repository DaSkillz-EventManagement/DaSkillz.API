using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Advertisement
    {
        public Advertisement()
        {
            Events = new HashSet<Event>();
        }

        public Guid EventId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public string Status {  get; set; }


        public virtual ICollection<Event> Events { get; set; }
    }
}
