using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Coupon
    {
        public Coupon()
        {
            Events = new HashSet<Event>();
        }


        public int Id { get; set; }
        public long CreatedDate { get; set; }
        public long ExpiredDate { get; set; }
        public int NOAttempts { get; set; }
        public string DiscountType { get; set; }
        public decimal Value { get; set; }

        public virtual ICollection<Event> Events { get; set; }

    }
}
