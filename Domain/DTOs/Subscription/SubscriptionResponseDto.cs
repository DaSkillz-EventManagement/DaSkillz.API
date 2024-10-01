using Domain.DTOs.User.Response;

namespace Domain.DTOs.Subscription
{
    public class SubscriptionResponseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public UserResponseDto? User { get; set; }
    }
}
