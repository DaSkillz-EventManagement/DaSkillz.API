using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.User.Response
{
    public class AccountStatisticDto
    {
        public Guid UserId { get; set; }
        //public string Email { get; set; }
        public int NumOfPurchasing { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
