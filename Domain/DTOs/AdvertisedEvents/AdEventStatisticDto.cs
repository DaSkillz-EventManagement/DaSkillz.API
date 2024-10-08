using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AdvertisedEvents
{
    public class AdEventStatisticDto
    {
        
        public Guid EventId { get; set; }
        public int NumOfPurchasing { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
