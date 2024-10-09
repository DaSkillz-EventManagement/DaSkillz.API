

namespace Domain.DTOs.Payment.Response;

public class TicketStatisticDto
{
    public int TotalTicket { get; set; }
    public int SuccessSoldTicket { get; set; }
    public int FailedSoldTicket { get; set; }
    public int ProcessingTicket {  get; set; }
    public double TotalRevenue { get; set; }
}
