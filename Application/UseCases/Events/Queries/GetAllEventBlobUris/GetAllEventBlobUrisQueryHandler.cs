using Application.Abstractions.Caching;
using Application.UseCases.Events.Command.CreateEvent;
using Azure;
using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetAllEventBlobUris
{
    public class GetAllEventBlobUrisQueryHandler : IRequestHandler<GetAllEventBlobUrisQuery, Dictionary<string, List<string>>>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IRedisCaching _redisCaching;

        public GetAllEventBlobUrisQueryHandler(IEventRepository eventRepo, IRedisCaching redisCaching)
        {
            _eventRepo = eventRepo;
            _redisCaching = redisCaching;
        }

        public async Task<Dictionary<string, List<string>>> Handle(GetAllEventBlobUrisQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetAllEventBlob";
            var cachedDataString = await _redisCaching.GetAsync<Dictionary<string, List<string>>>(cacheKey);
            if(cachedDataString != null)
            {
                return cachedDataString;
            }

            Event eventData = await _eventRepo.GetById(request.EventId);
            List<string> blobUris = new List<string>();
            List<string> eventTheme = new List<string>();
            if (eventData != null)
            {
                eventTheme.Add(eventData.Image!);
                foreach (var item in eventData.Logos)
                {
                    blobUris.Add(item.LogoUrl!);
                }
            }
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>
            {
                { "event avatar", eventTheme },
                { "event sponsor logos", blobUris }
            };
           
            await _redisCaching.SetAsync(cacheKey, result, 10);
            
            
            return result;
        }
    }
}
