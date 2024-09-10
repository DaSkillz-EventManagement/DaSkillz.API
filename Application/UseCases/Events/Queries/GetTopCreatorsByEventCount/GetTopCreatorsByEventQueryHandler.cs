using Application.UseCases.Events.Queries.GetEventParticipatedByUser;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetTopCreatorsByEventCount
{
    public class GetTopCreatorsByEventQueryHandler : IRequestHandler<GetTopCreatorsByEventQuery, List<EventCreatorLeaderBoardDto>>
    {
        private readonly IEventRepository _eventRepo;

        public GetTopCreatorsByEventQueryHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<List<EventCreatorLeaderBoardDto>> Handle(GetTopCreatorsByEventQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepo.GetTop10CreatorsByEventCount();
        }
    }
}
