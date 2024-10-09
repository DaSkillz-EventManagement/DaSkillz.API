using Application.Abstractions.Caching;
using Application.ResponseMessage;
using Azure;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.Quiz.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Admin.Queries.TicketStatistic;

public class TicketStatisticHandler : IRequestHandler<TicketStatisticQuery, APIResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IRedisCaching _caching;
    public TicketStatisticHandler(ITransactionRepository transactionRepository, IRedisCaching caching)
    {
        _transactionRepository = transactionRepository;
        _caching = caching;
    }
    public async Task<APIResponse> Handle(TicketStatisticQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = "ticket_statistic_caching";
            var cachingData = await _caching.GetAsync<TicketStatisticDto>(cacheKey);
            if (cachingData != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = cachingData
                };
            }
            TicketStatisticDto response = await _transactionRepository.GetTicketStatistic();
            if (response != null)
            {
                await _caching.SetAsync(cacheKey, response, 5);
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response
            };

        }catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.Complete,
                Data = ex.Message
            };
        }
    }
}
