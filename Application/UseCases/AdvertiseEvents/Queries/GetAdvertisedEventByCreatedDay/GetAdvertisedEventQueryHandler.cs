using Application.ResponseMessage;
using Domain.DTOs.Events.ResponseDto;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

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
            listEventEntity = listEventEntity.Where(e => !e.Status.Equals(EventStatus.Deleted.ToString())).ToList();
            var listEventResponse = listEventEntity.Select(_eventRepository.ToResponseDto).ToList();

            //var finalList = new List<EventResponseDto>();
            //foreach( var item in listEventResponse )
            //{
            //    if (item.Status.Equals(EventStatus.NotYet.ToString()))
            //    {
            //        finalList.Add(item);
            //    }
            //}


            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.Complete;
            response.Data = listEventResponse;
            return response;
        }
    }
}
