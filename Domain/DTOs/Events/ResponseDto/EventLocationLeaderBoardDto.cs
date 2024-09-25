namespace Domain.DTOs.Events.ResponseDto
{
    public class EventLocationLeaderBoardDto
    {
        public int totalevent { get; set; }
        //public string city { get; set; }
        public string Location { get; set; }
        public string? LocationUrl { get; set; } = "";
        public string? LocationCoord { get; set; } = "";
        public string? LocationAddress { get; set; } = "";
        public string? LocationId { get; set; } = "";
    }
}
