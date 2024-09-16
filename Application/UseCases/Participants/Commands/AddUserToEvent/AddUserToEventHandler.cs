using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using Application.ResponseMessage;
using System.Net;
using Domain.Entities;
using Domain.Enum.Participant;

namespace Application.UseCases.Participants.Commands.AddUserToEventCommand
{
    public class AddUserToEventHandler : IRequestHandler<AddUserToEventCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddUserToEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(AddUserToEventCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.UserId, request.RegisterEventModel.EventId);
            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            isOwner = await _eventRepo.IsOwner(request.UserId, request.RegisterEventModel.EventId);

            if (isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.HostCannotRegister,
                    Data = null
                };
            }
            Participant participant = new()
            {
                UserId = request.RegisterEventModel.UserId,
                EventId = request.RegisterEventModel.EventId,
                RoleEventId = request.RegisterEventModel.RoleEventId,
                CreatedAt = DateTime.Now,
                IsCheckedMail = false,
                Status = ParticipantStatus.Confirmed.ToString()
            };

            await _participantRepository.UpSert(participant);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                //_sendMailTask.SendMailTicket(registerEventModel);

                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.Created,
                    Message = MessageCommon.SavingSuccesfully,
                    Data = request.RegisterEventModel
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageCommon.SavingFailed,
                Data = request.RegisterEventModel
            };
        }
    }
}
