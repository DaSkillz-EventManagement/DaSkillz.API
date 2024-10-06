using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Enum.AdvertisedEvents;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.Extensions.Azure;
using System.Net;

namespace Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent
{
    public class UseAdvertisedEventQueryHandler : IRequestHandler<UseAdvertisedEventQuery, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPriceRepository _priceRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IAdvertisedEventRepository _advertisedEventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UseAdvertisedEventQueryHandler(IMapper mapper, IPriceRepository priceRepository, IEventRepository eventRepository, IAdvertisedEventRepository advertisedEventRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _priceRepository = priceRepository;
            _eventRepository = eventRepository;
            _advertisedEventRepository = advertisedEventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(UseAdvertisedEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var priceAd = await _priceRepository.GetPriceAdvertised();
            var existAd = await _advertisedEventRepository.GetLastestAdvertisedEvent(request.EventId);

            if(existAd != null)
            {
                if(existAd.EndDate < DateTimeHelper.GetCurrentTimeAsLong())
                {
                    
                    var isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
                    if (!isOwner)
                    {
                        response.StatusResponse = HttpStatusCode.BadRequest;
                        response.Message = MessageEvent.YouAreNotOwnerOfThisEvent;
                        response.Data = null;
                        return response;
                    }

                    var advertisedEntity = new AdvertisedEvent();
                    advertisedEntity.Id = Guid.NewGuid();
                    advertisedEntity.CreatedDate = DateTimeHelper.GetCurrentTimeAsLong();
                    advertisedEntity.EventId = request.EventId;
                    advertisedEntity.UserId = request.UserId;
                    advertisedEntity.PurchasedPrice = (decimal)priceAd.amount * request.numOfDate;
                    advertisedEntity.StartDate = DateTimeHelper.GetCurrentTimeAsLong();
                    advertisedEntity.Status = AdvertisedStatus.Active.ToString();

                    // Convert StartDate back to DateTimeOffset
                    DateTimeOffset startDate = DateTimeOffset.FromUnixTimeMilliseconds(advertisedEntity.StartDate);

                    // Add 3 days to the StartDate
                    DateTimeOffset endDate = startDate.AddDays(request.numOfDate);

                    // Convert the new EndDate back to Unix time (milliseconds)
                    advertisedEntity.EndDate = endDate.ToUnixTimeMilliseconds();

                    var result = _advertisedEventRepository.Add(advertisedEntity);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                    {
                        response.StatusResponse = HttpStatusCode.OK;
                        response.Message = MessageCommon.CreateSuccesfully;
                        response.Data = result;
                    }
                }
                else
                {
                    response.StatusResponse = HttpStatusCode.OK;
                    response.Message = MessageEvent.TheTimeOfAdvertisingIsStillRemain;
                    response.Data = null;
                }
            } else
            {
                

                var advertisedEntity = new AdvertisedEvent();
                advertisedEntity.Id = new Guid();
                advertisedEntity.CreatedDate = DateTimeHelper.GetCurrentTimeAsLong();
                advertisedEntity.EventId = request.EventId;
                advertisedEntity.UserId = request.UserId;
                advertisedEntity.PurchasedPrice = (decimal)priceAd.amount * request.numOfDate;
                advertisedEntity.StartDate = DateTimeHelper.GetCurrentTimeAsLong();
                advertisedEntity.Status = AdvertisedStatus.Active.ToString();

                // Convert StartDate back to DateTimeOffset
                DateTimeOffset startDate = DateTimeOffset.FromUnixTimeMilliseconds(advertisedEntity.StartDate);

                // Add 3 days to the StartDate
                DateTimeOffset endDate = startDate.AddDays(3);

                // Convert the new EndDate back to Unix time (milliseconds)
                advertisedEntity.EndDate = endDate.ToUnixTimeMilliseconds();

                var result = _advertisedEventRepository.Add(advertisedEntity);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    response.StatusResponse = HttpStatusCode.OK;
                    response.Message = MessageCommon.CreateSuccesfully;
                    response.Data = result;
                }
            }
            

            return response;
        }
    }
}
