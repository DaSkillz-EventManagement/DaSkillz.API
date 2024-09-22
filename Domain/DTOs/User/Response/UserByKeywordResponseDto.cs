namespace Domain.DTOs.User.Response
{
    public class UserByKeywordResponseDto
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool? IsPremiumUser { get; set; } = false;
    }
}
