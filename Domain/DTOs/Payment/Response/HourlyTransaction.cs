using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Payment.Response
{
    public class HourlyTransaction
    {
        public string Date { get; set; }
        public string Hour { get; set; }
        public string TotalAmount { get; set; }
        public int SubscriptionType { get; set; }
        public Dictionary<string, string> TotalsByType { get; set; } = new Dictionary<string, string>();
    }
}
