using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Coupons
{
    public class CouponEventDto
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }
}
