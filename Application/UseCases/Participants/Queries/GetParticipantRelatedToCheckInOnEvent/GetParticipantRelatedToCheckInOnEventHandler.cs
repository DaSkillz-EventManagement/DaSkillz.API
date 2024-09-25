using AutoMapper;
using Domain.DTOs.ParticipantDto;
using Domain.Enum.Participant;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Participants.Queries.GetParticipantRelatedToCheckInOnEvent
{
    public class GetParticipantRelatedToCheckInOnEventHandler : IRequestHandler<GetParticipantRelatedToCheckInOnEventQueries, PagedList<ParticipantDto>?>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public GetParticipantRelatedToCheckInOnEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<ParticipantDto>?> Handle(GetParticipantRelatedToCheckInOnEventQueries request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.EventId, request.UserId);
            if (!isOwner)
            {
                return null;
            }

            var participants = await _participantRepository.GetAll(p => p.EventId.Equals(request.EventId), request.page, request.eachPage, ParticipantSortBy.CheckedIn.ToString());

            return _mapper.Map<PagedList<ParticipantDto>>(participants);
        }
    }
}
