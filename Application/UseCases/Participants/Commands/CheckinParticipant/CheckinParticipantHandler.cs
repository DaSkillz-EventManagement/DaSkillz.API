using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum.Events;
using Elastic.Clients.Elasticsearch.Security;
using System.Text.Json;

namespace Application.UseCases.Participants.Commands.CheckinParticipant
{
    public class CheckinParticipantHandler : IRequestHandler<CheckinParticipantCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CheckinParticipantHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(CheckinParticipantCommand request, CancellationToken cancellationToken)
        {
            if (await _participantRepository.IsRole(request.UserId, request.EventId, EventRole.CheckingStaff))
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.YouAreNotStaff,
                    Data = null
                };
            }
            var participant = await _participantRepository.GetParticipant(request.UserId, request.EventId);

            if (participant == null)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageParticipant.CheckInUserFailed,
                    Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
                };
            }

            participant.CheckedIn = DateTime.Now;

            await _participantRepository.Update(participant);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageParticipant.CheckInUserSuccess,
                    Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.InternalServerError,
                Message = MessageCommon.ServerError,
                Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
            };
        }
    }
}
