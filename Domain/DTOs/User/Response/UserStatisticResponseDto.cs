using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.User.Response
{
    public class UserStatisticResponseDto
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Status { get; set; } = null!;
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Avatar { get; set; }
        public bool? IsPremiumUser { get; set; } = false;
        public int NumOfPurchasing { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
