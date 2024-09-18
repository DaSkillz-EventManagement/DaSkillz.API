using Application.Abstractions.Email;
using Infrastructure.ExternalServices.Email.Setting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.ExternalServices.Email
{

    public class EmailServices : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        public EmailServices(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }

        public async Task SendEmailAsync(string recipientEmail, string username, string otp, string path)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "test";

            // Load HTML content from the file
            var htmlContent = await File.ReadAllTextAsync(path);

            // Replace placeholders with actual values
            var hrml = htmlContent.Replace("@Model.UserName", username)
                                          .Replace("@Model.OTP", otp);

            // Set the email body with HTML content
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = hrml
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Send the email using an SMTP client
            using (var smtpClient = new SmtpClient())
            {
                // SMTP server configuration (replace with your SMTP server details)
                var smtpServer = _emailSetting.SmtpServer;
                var smtpPort = _emailSetting.Port; // or 465 for SSL
                var smtpUsername = _emailSetting.Username;
                var smtpPassword = _emailSetting.Password;

                // Connect to the SMTP server
                await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(smtpUsername, smtpPassword);

                // Send the email
                await smtpClient.SendAsync(message);

                // Disconnect from the SMTP server
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
