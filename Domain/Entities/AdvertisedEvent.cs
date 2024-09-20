using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdvertisedEvent
    {
        public AdvertisedEvent()
        {
            Events = new HashSet<Event>();
        }

        public Guid PurchaserId { get; set; }
        public Guid EventId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public decimal PurchasedPrice { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
