using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.AdvertisedEvents;
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
        private readonly IMapper _mapper;

        public GetFilteredAdvertisedByHostQueryHandler(IEventRepository eventRepository, IAdvertisedEventRepository advertisedEventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _advertisedEventRepository = advertisedEventRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFilteredAdvertisedByHostQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var list = await _advertisedEventRepository.GetFilteredAdvertisedByHost(request.UserId, request.Status);

            var resultList = _mapper.Map<List<AdvertisedEventDto>>(list);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = resultList;
            return response;

        }
    }
}
