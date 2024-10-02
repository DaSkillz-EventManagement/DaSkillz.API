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
        private readonly IUserRepository _userRepository;
        private readonly ICertificateRepository _certificateRepository;

        public CreateCertificateCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, ICertificateRepository certificateRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _certificateRepository = certificateRepository;
        }

        public async Task<APIResponse> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
        {
            if (request.UserIds == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = MessageCommon.CreateSuccesfully
                };
            }
                
            var certificatesToAdd = new List<Domain.Entities.Certificate>();

            foreach (var userid in request.UserIds)
            {
                var certificate = new Domain.Entities.Certificate
                {
                    UserId = userid,
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
