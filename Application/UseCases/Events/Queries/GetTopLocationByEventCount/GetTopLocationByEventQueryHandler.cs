using Application.UseCases.Events.Queries.GetTopCreatorsByEventCount;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
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

        public GetTopLocationByEventQueryHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<List<EventLocationLeaderBoardDto>> Handle(GetTopLocationByEventQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepo.GetTop10LocationByEventCount();
        }
    }
}
