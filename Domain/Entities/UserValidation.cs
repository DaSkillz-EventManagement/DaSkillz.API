namespace Domain.Entities
{
    public partial class UserValidation
    {
        public Guid UserId { get; set; }
        public string? Otp { get; set; }
        public string? VerifyToken { get; set; }
    }
}
