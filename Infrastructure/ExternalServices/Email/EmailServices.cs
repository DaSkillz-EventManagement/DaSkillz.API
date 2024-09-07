using Application.Abstractions.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.ExternalServices.Email
{
    public class EmailServices : IEmailService
    {
        public async Task SendEmailAsync(string recipientEmail, string username, string otp)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Daskillz", "daskillz45@gmail.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "test";

            // Load HTML content from the file
            var htmlContent = await File.ReadAllTextAsync("./template/VerifyWithOTP.cshtml");

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
                var smtpServer = "smtp.gmail.com";
                var smtpPort = 587; // or 465 for SSL
                var smtpUsername = "daskillz45@gmail.com";
                var smtpPassword = "knoxfxtwqdyvpccl";

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
