using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId
{
    public class GetSponsorRequestsByEventIdHandler : IRequestHandler<GetSponsorRequestsByEventIdQueries, PagedList<SponsorEventDto>?>
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
        public async Task<PagedList<SponsorEventDto>?> Handle(GetSponsorRequestsByEventIdQueries request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return null;
            }
            var list = await _sponsorEventRepository.GetSponsorEvents(request.SponsorFilter);
            int count = await _sponsorEventRepository.GetSponsorEventsCount(request.SponsorFilter);
            List<SponsorEventDto> response = ToSponsorEventDto(list);
            PagedList<SponsorEventDto> pages = new PagedList<SponsorEventDto>(response, count, request.SponsorFilter.Page, request.SponsorFilter.EachPage);
            return pages;
        }
        private List<SponsorEventDto> ToSponsorEventDto(List<SponsorEvent> list)
        {
            List<SponsorEventDto> result = new List<SponsorEventDto>();
            foreach (var item in list) 
            {
                SponsorEventDto temp = _mapper.Map<SponsorEventDto>(item);
                temp.FullName = item.User!.FullName!;
                temp.Email = item.User!.Email!;
                result.Add(temp);
            }
            return result;
        }
    }
}
