using Application.ResponseMessage;
using Domain.DTOs.ParticipantDto;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.UseCases.Participants.Queries.GetParticipantOnEvent
{
    internal class GetParticipantOnEventHandler : IRequestHandler<GetParticipantOnEventQueries, PagedList<ParticipantEventDto>?>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        
        public GetParticipantOnEventHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<ParticipantEventDto>?> Handle(GetParticipantOnEventQueries request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.FilterParticipant.EventId, request.UserId);
            if (!isOwner)
            {
                return null;
            }
            var participants = await _participantRepository.FilterDataParticipant(request.FilterParticipant);

            return _mapper.Map<PagedList<ParticipantEventDto>>(participants);
        }
    }
}
