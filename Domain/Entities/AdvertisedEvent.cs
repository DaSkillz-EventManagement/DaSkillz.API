﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdvertisedEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public long CreatedDate {  get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public string Status {  get; set; }
        public decimal PurchasedPrice { get; set; }

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
