using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Participants.Commands.ApproveInvite
{
    public class ApproveInviteHandler : IRequestHandler<ApproveInviteCommand, APIResponse>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveInviteHandler(IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
        {
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
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
