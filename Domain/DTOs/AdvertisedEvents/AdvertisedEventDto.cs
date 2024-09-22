using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AdvertisedEvents
{
    public class AdvertisedEventDto
    {
        public Guid PurchaserId { get; set; }
        public Guid EventId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public decimal PurchasedPrice { get; set; }
    }
}
