using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;

namespace Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string recipientEmail, string username, string otp, string path);
        Task SendEmailAsync(string recipientEmail, string username, string otp, string path);
        Task SendEmailTicket(string template, string title, TicketModel ticket);

    }
}
