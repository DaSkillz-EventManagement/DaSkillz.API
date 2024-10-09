using Application.ResponseMessage;
using Azure;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Admin.Queries.TicketStatistic;

public class TicketStatisticHandler : IRequestHandler<TicketStatisticQuery, APIResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    public TicketStatisticHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<APIResponse> Handle(TicketStatisticQuery request, CancellationToken cancellationToken)
    {
        try
        {
            TicketStatisticDto response = await _transactionRepository.GetTicketStatistic();
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
