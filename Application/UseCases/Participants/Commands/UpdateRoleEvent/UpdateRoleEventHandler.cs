using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseMessage;
using Domain.DTOs.ParticipantDto;
using System.Net;

namespace Application.UseCases.Participants.Commands.UpdateRoleEventCommand
{
    public class UpdateRoleEventHandler : IRequestHandler<UpdateRoleEventCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoleEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(UpdateRoleEventCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.RegisterEvent.EventId, request.ExecutorId);

            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            var participant = await _participantRepository.GetParticipant(request.RegisterEvent.UserId, request.RegisterEvent.EventId);

            if (participant == null)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = request.RegisterEvent
                };
            }

            participant.RoleEventId = request.RegisterEvent.RoleEventId;

            await _participantRepository.Update(participant);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.UpdateSuccesfully,
                    Data = request.RegisterEvent
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageCommon.UpdateFailed,
                Data = request.RegisterEvent
            };
        }
    }
}
