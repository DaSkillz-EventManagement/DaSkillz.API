using AutoMapper;
using Domain.DTOs.Events;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Command.GetUserPastAndIncomingEvent
{
    public class GetUserPastAndIncomingEventCommandHandler : IRequestHandler<GetUserPastAndIncomingEventCommand, Dictionary<string, List<EventPreviewDto>>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetUserPastAndIncomingEventCommandHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }


        public async Task<Dictionary<string, List<EventPreviewDto>>> Handle(GetUserPastAndIncomingEventCommand request, CancellationToken cancellationToken)
        {


            List<Event> pastEvent = await _eventRepo.UserPastEvents(request.UserId);
            List<Event> incoming = await _eventRepo.UserIncomingEvents(request.UserId);
            Dictionary<string, List<EventPreviewDto>> response = new Dictionary<string, List<EventPreviewDto>>
            {
                { "IncomingEvent", _mapper.Map<List<EventPreviewDto>>(incoming) },
                { "PastEvent", _mapper.Map<List<EventPreviewDto>>(pastEvent) }
            };
            return response;
        }
    }
}
