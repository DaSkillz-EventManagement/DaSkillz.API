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
        private readonly IUserRepository _userRepository;

        public GetAdvertisedInfoByEventQueryHandler(IEventRepository eventRepository, IAdvertisedEventRepository advertisedEventRepository, IMapper mapper, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _advertisedEventRepository = advertisedEventRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<APIResponse> Handle(GetAdvertisedInfoByEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
            var isAdmin = await _userRepository.IsAdmin(request.UserId);
            if (!isOwner || !isAdmin)
            {
                
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageEvent.UserNotAllowToSee;
                response.Data = null;
                return response;
            }
            var existAd = await _advertisedEventRepository.GetAdvertisedByEventId(request.EventId);
            var dto = _mapper.Map<List<AdvertisedEventDto>>(existAd);

            //var dto = new AdvertisedEventDto();
            //dto.UserId = existAd.UserId;
            //dto.EventId = existAd.EventId;
            //dto.CreatedDate = existAd.CreatedDate;
            //dto.EndDate = existAd.EndDate;
            //dto.StartDate = existAd.StartDate;
            //dto.PurchasedPrice = existAd.PurchasedPrice;
            if(dto != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = dto;
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
