using Application.UseCases.Events.Command.CreateEvent;
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

        public GetAllEventBlobUrisQueryHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<Dictionary<string, List<string>>> Handle(GetAllEventBlobUrisQuery request, CancellationToken cancellationToken)
        {
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
            return result;
        }
    }
}
