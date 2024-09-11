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
    public class GetSponsorRequestsHandler : IRequestHandler<GetSponsorRequestsQueries, PagedList<SponsorEvent>?>
    {
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IUserRepository _userRepository;
        public GetSponsorRequestsHandler(ISponsorEventRepository repository, IUserRepository userRepository)
        {
            _sponsorEventRepository = repository;
            _userRepository = userRepository;
        }
        public async Task<PagedList<SponsorEvent>?> Handle(GetSponsorRequestsQueries request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(request.UserId);
            if (userEntity == null)
            {
                return null;
            }
            var result = await _sponsorEventRepository.GetRequestSponsor(request.UserId, request.Status, request.Page,request.EachPage);
                return result;
        }
    }
}
