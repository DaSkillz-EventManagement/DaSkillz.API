using Application.ResponseMessage;
using Application.UseCases.Coupons.Queries.GetUsersByCoupon;
using Azure;
using Domain.Models.Response;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedEventByCreatedDay
{
    public class GetAdvertisedEventQueryHandler : IRequestHandler<GetAdvertisedEventQuery, APIResponse>
    {
        private readonly IAdvertisedEventRepository _advertisedEventRepository;
        private readonly IEventRepository _eventRepository;

        public GetAdvertisedEventQueryHandler(IAdvertisedEventRepository advertisedEventRepository, IEventRepository eventRepository)
        {
            _advertisedEventRepository = advertisedEventRepository;
            _eventRepository = eventRepository;
        }

        public async Task<APIResponse> Handle(GetAdvertisedEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var listEventId = await _advertisedEventRepository.GetListAdvertisedEventId();
            var listEventEntity = await _eventRepository.GetListEventsByListId(listEventId);

            var listEventResponse = listEventEntity.Select(_eventRepository.ToResponseDto).ToList();

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.Complete;
            response.Data = listEventResponse;
            return response;
        }
    }
}
