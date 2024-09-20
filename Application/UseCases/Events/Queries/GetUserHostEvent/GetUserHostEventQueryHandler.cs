using Application.Abstractions.Caching;
using Application.UseCases.Events.Queries.GetTopLocationByEventCount;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetUserHostEvent
{
    public class GetUserHostEventQueryHandler : IRequestHandler<GetUserHostEventQuery, List<EventPreviewDto>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetUserHostEventQueryHandler(IEventRepository eventRepo, IMapper mapper, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<List<EventPreviewDto>> Handle(GetUserHostEventQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetUserHostEvent_{request.UserId}";
            var cachedDataString = await _redisCaching.GetAsync<List<EventPreviewDto>>(cacheKey);
            if (cachedDataString != null)
            {
                return cachedDataString;
            }

            var result = await _eventRepo.GetUserHostEvent(request.UserId);
            var eventPreview = await Task.WhenAll(result.Select(ToEventPreview));

            await _redisCaching.SetAsync(cacheKey, eventPreview, 10);
            return eventPreview.ToList();
        }

        private async Task<EventPreviewDto> ToEventPreview(Event entity)
        {

            EventPreviewDto response = new EventPreviewDto();
            response.EventId = entity.Id;
            response.EventName = entity.EventName;
            response.Location = entity.Location;
            response.Status = entity.Status;
            response.Image = entity.Image;
            response.StartDate = entity.StartDate;
            response.Host = await _eventRepo.GetHostInfo((Guid)entity.CreatedBy!);
            return response;
        }
    }
}
