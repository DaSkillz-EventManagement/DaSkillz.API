using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Coupons
{
    public class CouponDto
    {
        public long ExpiredDate { get; set; }
        public int NOAttempts { get; set; }
        public string DiscountType { get; set; }
        public decimal Value { get; set; }
    }
}
