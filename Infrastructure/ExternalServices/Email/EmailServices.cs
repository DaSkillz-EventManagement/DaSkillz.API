using Application.Abstractions.Email;
using Domain.DTOs.ParticipantDto;
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

        public async Task SendEmailVerifyAsync(string subject,string recipientEmail, string username, string otp, string path, string logoPath)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;

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

            if (File.Exists(logoPath))
                {
                    var logo = bodyBuilder.LinkedResources.Add(logoPath);
                    logo.ContentId = "logo3";  
                }
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

        public Task SendEmailAsync(string recipientEmail, string username, string otp, string path)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailTicket(string template, string title, TicketModel ticket)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", ticket.Email));
            message.Subject = "Participant";
            var htmlContent = await File.ReadAllTextAsync(template);
            var startDate = ticket.StartDate!.Value;
            var startDateMM = startDate.ToString("MMM");
            var startDateD = startDate.ToString("dd");
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlContent.Replace("@Model.Message", ticket.Message)
                                      .Replace("@Model.FullName", ticket.FullName)
                                      .Replace("@Model.EventName", ticket.EventName)
                                      .Replace("@Model.LogoEvent", ticket.LogoEvent)
                                      .Replace("@@Model.Avatar", ticket.Avatar)
                                      .Replace("@Model.UserId", ticket.UserId.ToString())
                                      .Replace("@Model.OrgainzerName", ticket.OrgainzerName)
                                      .Replace("@Model.StartDateMM", startDateMM)
                                      .Replace("@Model.StartDateD", startDateD)
                                      .Replace("@Model.StartEndDate", ticket.StartDate.ToString() + " - " + ticket.EndDate.ToString())
                                      .Replace("@Model.Time", ticket.Time!.ToString())
                                      .Replace("@Model.Button", ticket.TypeButton)
                                      .Replace("@Model.Location", ticket.Location)
                                      .Replace("@Model.LocationUrl", ticket.LocationUrl)
                                      .Replace("@Model.LocationAddress", ticket.LocationAddress)
                                      .Replace("@Model.EventId", ticket.EventId.ToString())
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
                /*var response = await _fluentEmail.To(ticket.Email)
                    .Subject(title)
                    .UsingTemplateFromFile(template, ticket, true)
                    .SendAsync();
                return response.Successful;*/
            }
        }
    }
}
