using Application.Abstractions.Email;
using Application.ExternalServices.BackgroundTák;
using Application.ExternalServices.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalServices.Email
{
    public class SendMailTask : ISendMailTask
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly string path = "./Views/Template/VerifyWithOTP.cshtml";

        public SendMailTask(IBackgroundTaskQueue taskQueue, IServiceScopeFactory serviceScopeFactory, IEventRepository eventRepository, IUserRepository userRepository)
        {
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
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

            //var participants = await _unitOfWork.ParticipantRepository.GetAll(p => p.EventId.Equals(eventId) && p.Status!.Equals(ParticipantStatus.Confirmed) && p.User.Status.Equals(AccountStatus.Active));

            //if (!participants.Any())
            //{
            //    return;
            //}

            //var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            //foreach (var participant in participants)
            //{
            //    await _emailService.SendEmailTicket(MailConstant.ReminderMail.PathTemplate, MailConstant.ReminderMail.Title, new TicketModel()
            //    {
            //        EventId = participant.EventId,
            //        UserId = (Guid)participant.Event.CreatedBy!,
            //        Email = participant.User.Email,
            //        RoleEventId = (int)participant.RoleEventId!,
            //        FullName = participant.User.FullName,
            //        Avatar = participant.User.Avatar,
            //        EventName = participant.Event.EventName,
            //        Location = participant.Event.Location,
            //        LocationAddress = participant.Event.LocationAddress,
            //        LogoEvent = participant.Event.Image,
            //        OrgainzerName = participant.Event.CreatedByNavigation?.FullName,
            //        StartDate = participant.Event.StartDate,
            //        EndDate = participant.Event.StartDate.IsTheSameDate(participant.Event.EndDate) ? null : participant.Event.EndDate,
            //        Time = DateTimeHelper.GetTimeRange(participant.Event.StartDate, participant.Event.EndDate)
            //    });

            //    Console.WriteLine($"Send mail for {participant.UserId} complete!");
            //}

        }

        public async Task SendTicketForVisitor(RegisterEventModel registerEventModel)
        {
            var _unitOfWork = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

            var currentEvent = await _eventRepository.GetById(registerEventModel.EventId);

            if (currentEvent == null)
            {
                return;
            }

            var user = await _userRepository.GetById(registerEventModel.UserId);

            if (user == null)
            {
                return;
            }

            //var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            //await _emailService.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
            //{
            //    EventId = registerEventModel.EventId,
            //    UserId = (Guid)currentEvent.CreatedBy!,
            //    Email = user.Email,
            //    RoleEventId = registerEventModel.RoleEventId,
            //    FullName = user.FullName,
            //    Avatar = currentEvent?.CreatedByNavigation?.Avatar,
            //    EventName = currentEvent?.EventName,
            //    Location = currentEvent?.Location,
            //    LocationAddress = currentEvent?.LocationAddress,
            //    LogoEvent = currentEvent?.Image,
            //    OrgainzerName = currentEvent?.CreatedByNavigation?.FullName,
            //    StartDate = currentEvent!.StartDate,
            //    EndDate = currentEvent!.StartDate.IsTheSameDate(currentEvent!.EndDate) ? null : currentEvent!.EndDate,
            //    Time = DateTimeHelper.GetTimeRange(currentEvent.StartDate, currentEvent.EndDate),
            //    Message = TicketMailConstant.MessageMail.ElementAt(registerEventModel.RoleEventId),
            //    TypeButton = Utilities.GetTypeButton(registerEventModel.RoleEventId),
            //});

            Console.WriteLine("Send mail complete!");
        }
    }
}
