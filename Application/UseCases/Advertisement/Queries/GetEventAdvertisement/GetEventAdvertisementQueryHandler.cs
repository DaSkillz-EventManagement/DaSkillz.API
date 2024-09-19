using Application.Abstractions.Caching;
using Application.UseCases.Events.Queries.GetEventByTag;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Advertisement.Queries.GetEventAdvertisement
{
    public class GetEventAdvertisementQueryHandler : IRequestHandler<GetEventAdvertisementQuery, List<EventResponseDto>>
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetEventAdvertisementQueryHandler(IAdvertisementRepository advertisementRepository, IEventRepository eventRepository, IMapper mapper, IRedisCaching redisCaching)
        {
            _advertisementRepository = advertisementRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventResponseDto>> Handle(GetEventAdvertisementQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetEventAdvertisement";
            var cachedDataString = await _redisCaching.GetAsync<List<EventResponseDto>>(cacheKey);
            if(cachedDataString != null)
            {
                return cachedDataString;
            }

            var result = await _advertisementRepository.GetAllEventIdsAsync();
            var listEvent = await _eventRepository.GetEventsByIdsAsync(result);
            var response = _mapper.Map<List<EventResponseDto>>(listEvent);
            await _redisCaching.SetAsync(cacheKey, response, 10);

            return response;
        }
    }
}
