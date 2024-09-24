using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent
{
    public class UseAdvertisedEventQueryHandler : IRequestHandler<UseAdvertisedEventQuery, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPriceRepository _priceRepository;
        private readonly IEventRepository _eventRepository;

        public UseAdvertisedEventQueryHandler(IMapper mapper, IPriceRepository priceRepository, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _priceRepository = priceRepository;
            _eventRepository = eventRepository;
        }

        public async Task<APIResponse> Handle(UseAdvertisedEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();

            var isOwner = await _eventRepository.IsOwner(request.UserId, request.AdvertisedEventDto.EventId);
            if (isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageEvent.YouAreNotOwnerOfThisEvent;
                response.Data = null;
                return response;
            }
            var advertisedEntity = _mapper.Map<AdvertisedEvent>(request);
            advertisedEntity.CreatedDate = DateTimeHelper.GetCurrentTimeAsLong();
            var priceAd = _priceRepository.GetAllPriceAdvertised();

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.CreateSuccesfully;
            response.Data = priceAd;

            return response;
        }
    }
}
