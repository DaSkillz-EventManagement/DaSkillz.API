using Application.ResponseMessage;
using Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent;
using AutoMapper;
using Domain.DTOs.AdvertisedEvents;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedInfoByEvent
{
    public class GetAdvertisedInfoByEventQueryHandler : IRequestHandler<GetAdvertisedInfoByEventQuery, APIResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAdvertisedEventRepository _advertisedEventRepository;
        private readonly IMapper _mapper;

        public GetAdvertisedInfoByEventQueryHandler(IEventRepository eventRepository, IAdvertisedEventRepository advertisedEventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _advertisedEventRepository = advertisedEventRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAdvertisedInfoByEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageEvent.YouAreNotOwnerOfThisEvent;
                response.Data = null;
                return response;
            }
            var existAd = await _advertisedEventRepository.GetAdvertisedByEventId(request.EventId);
            var dto = _mapper.Map<AdvertisedEventDto>(existAd);
            if(dto != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = existAd;
            } else
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = null;
            }
            
            return response;
        }
    }
}
