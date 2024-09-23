using Application.ExternalServices.Mail;
using Application.ResponseMessage;
using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Enum.Participant;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;
using System.Transactions;


namespace Application.UseCases.Participants.Commands.RegisterEventCommand;

public class RegisterEventHandler : IRequestHandler<RegisterEventCommand, APIResponse>
{
    private readonly IEventRepository _eventRepo;
    private readonly IParticipantRepository _participantRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
    {
        _eventRepo = eventRepo;
        _participantRepository = participantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
    {
        if (request.TransactionId != null && !Guid.TryParse(request.TransactionId.ToString(), out _))
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageParticipant.TransactionIsNotValid,
                Data = null
            };
        }
        var isOwner = await _eventRepo.IsOwner(request.EventId, request.UserId);

        if (isOwner)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageParticipant.HostCannotRegister,
                Data = null
            };
        }
        
        var isExistedOnEvent = await _participantRepository.IsExistedOnEvent(request.UserId, request.EventId);
        if (isExistedOnEvent)
        {
            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.Conflict,
                Message = MessageParticipant.ExistedOnEvent,
                Data = new { request.UserId, request.EventId }
            };
        }
        var registerEventModel = new RegisterEventModel
        {
            UserId = request.UserId,
            EventId = request.EventId,
            RoleEventId = (int)EventRole.Visitor + 1
        };
        var currentEvent = await _eventRepo.GetById(registerEventModel.EventId);

        Participant participant = new()
        {
            UserId = registerEventModel.UserId,
            EventId = registerEventModel.EventId,
            RoleEventId = registerEventModel.RoleEventId,
            CreatedAt = DateTime.Now,
            IsCheckedMail = false,
            Status = currentEvent!.Approval ? ParticipantStatus.Pending.ToString() : ParticipantStatus.Confirmed.ToString()
        };

        await _participantRepository.Add(participant);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            /*if (!currentEvent!.Approval)
            {
                _sendMailTask.SendMailTicket(registerEventModel);
            }*/

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.Created,
                Message = MessageCommon.SavingSuccesfully,
                Data = registerEventModel
            };
        }

        return new APIResponse()
        {
            StatusResponse = HttpStatusCode.NotModified,
            Message = MessageCommon.SavingFailed,
            Data = registerEventModel
        };
    }
}
