namespace Domain.DTOs.User.Response
{
    public class CreatedByUserDto
    {
        public string? Name { get; set; }
        public Guid? Id { get; set; }
        public string? avatar { get; set; }
        public string email { get; set; } = string.Empty;
    }
}
