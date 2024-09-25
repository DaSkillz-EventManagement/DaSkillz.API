using Domain.DTOs.ParticipantDto;

namespace Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailVerifyAsync(string subject, string recipientEmail, string username, string otp, string path, string logoPath);
        Task SendEmailTicket(string template, string title, TicketModel ticket);

    }
}
