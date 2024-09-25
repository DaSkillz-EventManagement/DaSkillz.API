namespace Domain.DTOs.Events.ResponseDto
{
    public class EventCreatorLeaderBoardDto
    {
        public Guid? userId { get; set; }
        public string? FullName { get; set; } = "";
        public string? Avatar { get; set; } = "";
        public int totalevent { get; set; }
    }
}
