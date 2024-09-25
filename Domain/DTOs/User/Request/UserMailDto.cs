namespace Domain.DTOs.User.Request
{
    public class UserMailDto
    {
        public string UserName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string OTP { get; set; } = null!;
    }
}
