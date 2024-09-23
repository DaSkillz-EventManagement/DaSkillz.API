using Application.Abstractions.Email;
using Application.ExternalServices.BackgroundTák;
using Application.ExternalServices.Mail;
using Application.Helper;
using Domain.Constants.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;
using Domain.Entities;
using Domain.Enum.Participant;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalServices.Email
{
    public class SendMailTask : ISendMailTask
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly string path = "./Views/Template/VerifyWithOTP.cshtml";

        public SendMailTask(IBackgroundTaskQueue taskQueue, IServiceScopeFactory serviceScopeFactory, IEventRepository eventRepository, IUserRepository userRepository, 
            IParticipantRepository participantRepository)
        {
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _participantRepository = participantRepository;
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

        //public void SendMailVerify(UserMailDto userMailDto)
        //{
        //    _taskQueue.QueueBackgroundWorkItem(async token =>
        //    {
        //        await VerifyMail(userMailDto);
        //    });
        //}


        //public async Task VerifyMail(UserMailDto userMailDto)
        //{
        //    var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
        //    await _emailService.SendEmailAsync(userMailDto.Email, userMailDto.UserName, userMailDto.OTP, path);

        //    Console.WriteLine("Send mail complete!");

        //}


        public async Task ReminderEvent(Guid eventId)
        {
            var _unitOfWork = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

            var participants = await _participantRepository.GetAll(p => p.EventId.Equals(eventId) && p.Status!.Equals(ParticipantStatus.Confirmed) && p.User.Status.Equals(AccountStatus.Active));

            if (!participants.Any())
            {
                return;
            }

            var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            foreach (var participant in participants)
            {
                await _emailService.SendEmailTicket(MailConstant.ReminderMail.PathTemplate, MailConstant.ReminderMail.Title, new TicketModel()
                {
                    EventId = participant.EventId,
                    UserId = (Guid)participant.Event.CreatedBy!,
                    Email = participant.User.Email,
                    RoleEventId = (int)participant.RoleEventId!,
                    FullName = participant.User.FullName,
                    Avatar = participant.User.Avatar,
                    EventName = participant.Event.EventName,
                    Location = participant.Event.Location,
                    LocationAddress = participant.Event.LocationAddress,
                    LogoEvent = participant.Event.Image,
                    OrgainzerName = participant.Event.CreatedByNavigation?.FullName,
                    StartDate = DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.StartDate).DateTime,
                    EndDate = DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.StartDate).DateTime
                    .IsTheSameDate(DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.EndDate).DateTime) ? null : DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.EndDate).DateTime,
                    Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(participant.Event.EndDate).DateTime)
                });

                Console.WriteLine($"Send mail for {participant.UserId} complete!");
            }

        }

        public async Task SendTicketForVisitor(RegisterEventModel registerEventModel)
        {
            var _unitOfWork = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

            var currentEvent = await _eventRepository.GetById(registerEventModel.EventId);
            var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
            if (currentEvent == null)
            {
                return;
            }

            var user = await _userRepository.GetById(registerEventModel.UserId);

            if (user == null)
            {
                return;
            }

            var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            await _emailService.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
            {
                EventId = registerEventModel.EventId,
                UserId = (Guid)currentEvent.CreatedBy!,
                Email = user.Email,
                RoleEventId = registerEventModel.RoleEventId,
                FullName = user.FullName,
                Avatar = Owner!.Avatar!,
                EventName = currentEvent?.EventName,
                Location = currentEvent?.Location,
                LocationAddress = currentEvent?.LocationAddress,
                LogoEvent = currentEvent?.Image,
                OrgainzerName = currentEvent?.CreatedByNavigation?.FullName,
                StartDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.StartDate).DateTime,
                EndDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.EndDate).DateTime,
                Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.EndDate).DateTime),
                Message = TicketMailConstant.MessageMail.ElementAt(registerEventModel.RoleEventId),
                TypeButton = Utilities.GetTypeButton(registerEventModel.RoleEventId),
            });

            Console.WriteLine("Send mail complete!");
        }
    }
}
