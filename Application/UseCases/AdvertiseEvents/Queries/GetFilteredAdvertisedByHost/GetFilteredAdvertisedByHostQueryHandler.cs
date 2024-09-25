using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.AdvertiseEvents.Queries.GetFilteredAdveetisedByHost
{
    public class GetFilteredAdvertisedByHostQueryHandler : IRequestHandler<GetFilteredAdvertisedByHostQuery, APIResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAdvertisedEventRepository _advertisedEventRepository;

        public GetFilteredAdvertisedByHostQueryHandler(IEventRepository eventRepository, IAdvertisedEventRepository advertisedEventRepository)
        {
            _eventRepository = eventRepository;
            _advertisedEventRepository = advertisedEventRepository;
        }

        public async Task<APIResponse> Handle(GetFilteredAdvertisedByHostQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var list = await _advertisedEventRepository.GetFilteredAdvertisedByHost(request.UserId, request.Status);

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = list;
            return response;

        }
    }
}
