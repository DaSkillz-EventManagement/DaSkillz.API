﻿using AutoMapper;
using Domain.DTOs.Events;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetUserPastAndIncomingEvent
{
    public class GetUserPastAndIncomingEventQueryHandler : IRequestHandler<GetUserPastAndIncomingEventQuery, Dictionary<string, List<EventPreviewDto>>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public GetUserPastAndIncomingEventQueryHandler(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }


        public async Task<Dictionary<string, List<EventPreviewDto>>> Handle(GetUserPastAndIncomingEventQuery request, CancellationToken cancellationToken)
        {


            List<Event> pastEvent = await _eventRepo.UserPastEvents(request.UserId);
            List<Event> incoming = await _eventRepo.UserIncomingEvents(request.UserId);

            Dictionary<string, List<EventPreviewDto>> response = new Dictionary<string, List<EventPreviewDto>>
            {
                { "IncomingEvent", incoming.Select(_eventRepo.ToEventPreview).ToList() },
                { "PastEvent", pastEvent.Select(_eventRepo.ToEventPreview).ToList() }
            };


            return response;
        }
    }
}
