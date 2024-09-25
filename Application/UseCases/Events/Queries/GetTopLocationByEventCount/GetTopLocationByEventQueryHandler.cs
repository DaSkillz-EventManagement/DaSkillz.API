using Application.Abstractions.Caching;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
using MediatR;

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
            return await _eventRepo.GetTop10LocationByEventCount();
        }
    }
}
