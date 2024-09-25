using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequests
{
    public class GetSponsorRequestsHandler : IRequestHandler<GetSponsorRequestsQueries, PagedList<SponsorEventDto>?>
    {
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetSponsorRequestsHandler(ISponsorEventRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _sponsorEventRepository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<SponsorEventDto>?> Handle(GetSponsorRequestsQueries request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(request.UserId);
            if (userEntity == null)
            {
                return null;
            }
            var result = await _sponsorEventRepository.GetRequestSponsor(request.UserId, request.Status, request.Page,request.EachPage);
                return _mapper.Map<PagedList<SponsorEventDto>>(result);
        }
    }
}
