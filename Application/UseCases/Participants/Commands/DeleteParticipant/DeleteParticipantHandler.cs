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
using Domain.Entities;
using System.Net;
using Elastic.Clients.Elasticsearch.Security;
using System.Text.Json;

namespace Application.UseCases.Participants.Commands.DeleteParticipantCommand
{
    public class DeleteParticipantHandler : IRequestHandler<DeleteParticipantCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteParticipantHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.ExcecutorId, request.EventId);

            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            if (!(await _participantRepository.IsExistedOnEvent(request.UserId, request.EventId)))
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
                };
            }

            await _participantRepository.Delete(request.UserId, request.EventId);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageCommon.DeleteFailed,
                Data = JsonSerializer.Serialize(new { request.UserId, request.EventId })
            };
        }
    }
}
