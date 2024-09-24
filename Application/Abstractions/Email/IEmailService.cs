using Domain.DTOs.ParticipantDto;

namespace Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string recipientEmail, string username, string otp, string path);
        Task SendEmailAsync(string recipientEmail, string username, string otp, string path);
        Task SendEmailTicket(string template, string title, TicketModel ticket);

    }
}
