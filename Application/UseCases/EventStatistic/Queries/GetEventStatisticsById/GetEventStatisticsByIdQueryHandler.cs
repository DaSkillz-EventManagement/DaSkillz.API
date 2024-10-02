using Application.ResponseMessage;
using Application.UseCases.Events.Queries.GetEventByTag;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.UseCases.EventStatistic.Queries.GetEventStatisticsById
{
    public class GetEventStatisticsByIdQueryHandler : IRequestHandler<GetEventStatisticsByIdQuery, APIResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventStatisticsRepository _eventStatisticsRepository;

        public GetEventStatisticsByIdQueryHandler(IEventRepository eventRepository, IEventStatisticsRepository eventStatisticsRepository)
        {
            _eventRepository = eventRepository;
            _eventStatisticsRepository = eventStatisticsRepository;
        }

        public async Task<APIResponse> Handle(GetEventStatisticsByIdQuery request, CancellationToken cancellationToken)
        {
            bool isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
            var response = new APIResponse();
            if(isOwner)
            {
                var result = await _eventStatisticsRepository.GetById(request.EventId);
                if(result != null)
                {
                    response.Message = MessageCommon.Complete;
                    response.StatusResponse = HttpStatusCode.OK;
                    response.Data = result;
                } else
                {
                    response.Message = MessageCommon.NotFound;
                    response.StatusResponse = HttpStatusCode.NotFound;
                    response.Data = null;
                }
               
            } else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageEvent.OnlyHostCanUpdateEvent;
                response.Data = null;
            }
            return response;
        }
    }
}
