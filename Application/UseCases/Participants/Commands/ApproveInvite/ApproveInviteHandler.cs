using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;
using Domain.DTOs.ParticipantDto;
using Application.Helper;
using Domain.Constants.Mail;
using Application.Abstractions.Email;

namespace Application.UseCases.Participants.Commands.ApproveInvite
{
    public class ApproveInviteHandler : IRequestHandler<ApproveInviteCommand, APIResponse>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _sendMail;

        public ApproveInviteHandler(IParticipantRepository participantRepository, IUnitOfWork unitOfWork, IEventRepository eventRepo, 
            IUserRepository userRepository, IEmailService sendMail)
        {
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
            _eventRepo = eventRepo;
            _userRepository = userRepository;
            _sendMail = sendMail;
        }
        public async Task<APIResponse> Handle(ApproveInviteCommand request, CancellationToken cancellationToken)
        {
            var participant = await _participantRepository.GetParticipant(request.UserId, request.EventId);

            if (participant == null)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }

            participant.Status = request.status;

            await _participantRepository.Update(participant);
            var currentEvent = await _eventRepo.GetById(request.EventId);
            var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                #region sendMail
                    var user = await _userRepository.GetById(request.UserId);
                    await _sendMail.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
                    {
                        EventId = request.EventId,
                        UserId = (Guid)currentEvent.CreatedBy!,
                        Email = user!.Email,
                        RoleEventId = (int)participant.RoleEventId!,
                        FullName = user.FullName,
                        Avatar = Owner!.Avatar,
                        EventName = currentEvent?.EventName,
                        Location = currentEvent?.Location,
                        LocationUrl = currentEvent?.LocationUrl,
                        LocationAddress = currentEvent?.LocationAddress,
                        LogoEvent = currentEvent?.Image,
                        OrgainzerName = Owner.FullName,
                        StartDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.StartDate).DateTime,
                        EndDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.EndDate).DateTime,
                        Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.EndDate).DateTime),
                        Message = TicketMailConstant.MessageMail.ElementAt((int)participant.RoleEventId! - 1),
                        TypeButton = Utilities.GetTypeButton((int)participant.RoleEventId!),
                    });
                #endregion
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageParticipant.ProcessParticipant
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageParticipant.ProcessParticipantFailed
            };
        }
    }
}
