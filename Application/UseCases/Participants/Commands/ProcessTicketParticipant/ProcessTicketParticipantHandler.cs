using Application.Helper;
using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Participants.Commands.ProcessTicketParticipant
{
    public class ProcessTicketParticipantHandler : IRequestHandler<ProcessTicketParticipantCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessTicketParticipantHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(ProcessTicketParticipantCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.ParticipantTicket.EventId, request.UserId);
            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            if (!Utilities.IsValidParticipantStatus(request.ParticipantTicket.Status))
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.ParticipantStatusNotValid,
                    Data = null
                };
            }
            var participant = await _participantRepository.GetParticipant(request.ParticipantTicket.UserId, request.ParticipantTicket.EventId);

            if (participant == null)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }
            participant.Status = request.ParticipantTicket.Status;

            await _participantRepository.Update(participant);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
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
