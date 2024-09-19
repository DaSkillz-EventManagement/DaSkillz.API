using Application.Abstractions.Caching;
using Application.UseCases.Events.Queries.GetTopCreatorsByEventCount;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetTopLocationByEventCount
{
    public class GetTopLocationByEventQueryHandler : IRequestHandler<GetTopLocationByEventQuery, List<EventLocationLeaderBoardDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IRedisCaching _redisCaching;

        public GetTopLocationByEventQueryHandler(IEventRepository eventRepo, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventLocationLeaderBoardDto>> Handle(GetTopLocationByEventQuery request, CancellationToken cancellationToken)
        {
          
            var result = await _eventRepo.GetTop10LocationByEventCount();
            
            return result;
        }
    }
}
