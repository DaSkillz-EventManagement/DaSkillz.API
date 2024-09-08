using Application.Abstractions.Email;
using Application.ExternalServices.BackgroundTák;
using Application.ExternalServices.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Email
{
    public class SendMailTask : ISendMailTask
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMailTask(IBackgroundTaskQueue taskQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void SendMailReminder(Guid eventId)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await ReminderEvent(eventId);
            });
        }

        public void SendMailTicket(RegisterEventModel registerEventModel)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await SendTicketForVisitor(registerEventModel);
            });
        }

        public void SendMailVerify(UserMailDto userMailDto)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await VerifyMail(userMailDto);
            });
        }


        public async Task VerifyMail(UserMailDto userMailDto)
        {
            var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            var emailSent = await _emailService.SendEmailWithTemplate("Your OTP Code", userMailDto);

            if (emailSent)
            {
                Console.WriteLine("Send mail complete!");
            }

            Console.WriteLine("Send mail failed!");

        }
    }
}
