using Application.ResponseMessage;
using Application.UseCases.Subscriptions.Query;
using AutoMapper;
using Domain.DTOs.Certificate;
using Domain.DTOs.Subscription;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Certificate.Queries
{
    public class GetFilterCertificateHandler : IRequestHandler<GetFilterCertificate, APIResponse>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public GetFilterCertificateHandler(ICertificateRepository subscriptionRepository, IMapper mapper)
        {
            _certificateRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFilterCertificate request, CancellationToken cancellationToken)
        {
            var result = await _certificateRepository.GetFilteredCertificates(request.userId, request.eventId, request.issueDate);

            var mapResult = _mapper.Map<IEnumerable<CertificateResponseDto>>(result);
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = mapResult
            };
        }
    }
}
