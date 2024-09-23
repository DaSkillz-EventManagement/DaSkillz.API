namespace Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string recipientEmail, string username, string otp, string path);
    }
}
