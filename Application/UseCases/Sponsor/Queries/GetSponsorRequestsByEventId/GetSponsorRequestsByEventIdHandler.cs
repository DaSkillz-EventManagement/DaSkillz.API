using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId
{
    public class GetSponsorRequestsByEventIdHandler : IRequestHandler<GetSponsorRequestsByEventIdCommand, PagedList<SponsorEventDto>?>
    {
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public GetSponsorRequestsByEventIdHandler(ISponsorEventRepository repository, IMapper mapper, IUserRepository userRepository)
        {
            _sponsorEventRepository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<PagedList<SponsorEventDto>?> Handle(GetSponsorRequestsByEventIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return null;
            }
            var list = await _sponsorEventRepository.GetSponsorEvents(request.SponsorFilter);
            return _mapper.Map<PagedList<SponsorEventDto>>(list);
        }
    }
}
