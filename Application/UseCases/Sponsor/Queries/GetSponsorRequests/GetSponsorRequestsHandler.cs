using AutoMapper;
using Azure;
using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch.MachineLearning;
using Elastic.Clients.Elasticsearch.Security;
using Elastic.Clients.Elasticsearch.Snapshot;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int count = await _sponsorEventRepository.GetRequestSponsorCount(request.UserId, request.Status);
            List<SponsorEventDto> response = ToSponsorEventDto(result);
            PagedList<SponsorEventDto> pages = new PagedList<SponsorEventDto>(response, count, request.Page, request.EachPage);
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
