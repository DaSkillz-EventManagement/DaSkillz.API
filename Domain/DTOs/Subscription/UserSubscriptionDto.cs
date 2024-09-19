using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Subscription
{
    public class UserSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public bool IsActive { get; set; }
    }
}
