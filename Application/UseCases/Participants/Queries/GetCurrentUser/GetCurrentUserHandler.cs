using AutoMapper;
using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQueries, ParticipantEventDto>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        public GetCurrentUserHandler(IParticipantRepository participantRepository, IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }
        public async Task<ParticipantEventDto> Handle(GetCurrentUserQueries request, CancellationToken cancellationToken)
        {
            var user = await _participantRepository.GetDetailParticipant(request.UserId, request.EventId);

            return _mapper.Map<ParticipantEventDto>(user);
        }
    }
}
