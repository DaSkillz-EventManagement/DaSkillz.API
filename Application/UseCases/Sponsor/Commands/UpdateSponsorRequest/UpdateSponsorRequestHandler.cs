using Application.Abstractions.Email;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Enum.Participant;
using Application.Abstractions.Email;
using Domain.DTOs.ParticipantDto;
using Domain.Constants.Mail;
using Domain.Enum.Sponsor;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Sponsor.Commands.UpdateSponsorRequest;

public class UpdateSponsorRequestHandler : IRequestHandler<UpdateSponsorRequestCommand, APIResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IEmailService _sendMailTask;
    public UpdateSponsorRequestHandler(IMapper mapper, IUnitOfWork unitOfWork, ISponsorEventRepository repository, IEmailService emailService,
        IEventRepository eventRepository, IUserRepository userRepository, IParticipantRepository participantRepository)
    {
        _sponsorEventRepository = repository;
        _mapper = mapper;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _participantRepository = participantRepository;
        _sendMailTask = emailService;
    }
    public async Task<APIResponse> Handle(UpdateSponsorRequestCommand request, CancellationToken cancellationToken)
    {
        APIResponse response = new APIResponse();
        var userEntity = await _userRepository.GetUserByIdAsync(request.UserId);
        if (userEntity == null)
        {
            response.StatusResponse = HttpStatusCode.BadRequest;
            response.Message = MessageUser.UserNotFound;
            response.Data = null;
            return response;
        }

        var isOwner = await _eventRepository.IsOwner(request.SponsorRequestUpdateDto.EventId, request.UserId);

        if (!isOwner)
        {
            response.StatusResponse = HttpStatusCode.BadRequest;
            response.Message = MessageParticipant.NotOwner;
            response.Data = null;
            return response;
        }
        var sponsorRequest = await _sponsorEventRepository.CheckSponsorEvent(request.SponsorRequestUpdateDto.EventId, request.SponsorRequestUpdateDto.UserId);
        if (sponsorRequest != null)
        {
            sponsorRequest.Status = request.SponsorRequestUpdateDto.Status;
            sponsorRequest.UpdatedAt = DateTimeHelper.GetDateTimeNow();
            if (sponsorRequest.Status!.Equals(SponsorRequest.Confirmed.ToString()))
            {
                Participant participant = new()
                {
                    UserId = (Guid)sponsorRequest.UserId!,
                    EventId = (Guid)sponsorRequest.EventId!,
                    RoleEventId = (int)EventRole.Sponsor + 1,
                    CreatedAt = DateTime.Now,
                    IsCheckedMail = false,
                    Status = ParticipantStatus.Confirmed.ToString()
                };
                await _participantRepository.UpSert(participant);
            }
        }
        await _sponsorEventRepository.Update(sponsorRequest!);
        var currentEvent = await _eventRepository.GetById(request.SponsorRequestUpdateDto.EventId);
        var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            #region sendMail
                var user = await _userRepository.GetById(request.SponsorRequestUpdateDto.UserId);
                await _sendMailTask.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
                {
                    EventId = currentEvent.EventId,
                    UserId = (Guid)currentEvent.CreatedBy!,
                    Email = user!.Email,
                    RoleEventId = (int)EventRole.Sponsor,
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
                    Message = TicketMailConstant.MessageMail.ElementAt((int)EventRole.Sponsor),
                    TypeButton = Utilities.GetTypeButton((int)EventRole.Sponsor),
                });
            #endregion
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.UpdateSuccesfully,
                Data = sponsorRequest
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.UpdateFailed,
            Data = null
        };
    }
}
