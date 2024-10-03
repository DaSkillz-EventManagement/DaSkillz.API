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

namespace Application.UseCases.Certificate.Command
{
    public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IParticipantRepository _participantRepository;

        public CreateCertificateCommandHandler(IUnitOfWork unitOfWork, ICertificateRepository certificateRepository, IParticipantRepository participantRepository)
        {
            _unitOfWork = unitOfWork;
            _certificateRepository = certificateRepository;
            _participantRepository = participantRepository;
        }

        public async Task<APIResponse> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
        {
            if (request.UserIds == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = MessageCommon.CreateFailed
                };
            }
                
            var certificatesToAdd = new List<Domain.Entities.Certificate>();

            foreach (var userId in request.UserIds)
            {
                var exist = await _participantRepository.GetParticipant(userId, request.EventId);
                if (exist == null) continue;

                var certificate = new Domain.Entities.Certificate
                {
                    CertificateID = new Guid(),
                    UserId = userId,
                    EventId = request.EventId,
                    IssueDate = DateTime.UtcNow
                };

                certificatesToAdd.Add(certificate);
            }

            await _certificateRepository.AddRange(certificatesToAdd);
            var result = await _unitOfWork.SaveChangesAsync();
                return new APIResponse
                {
                    StatusResponse = (result > 0) ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.BadRequest,
                    Message = (result > 0) ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                };
        }
    }
}
