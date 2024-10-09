using Application.ResponseMessage;
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
        TicketStatisticDto response = await _transactionRepository.GetTicketStatistic();
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = response
        };
    }
}
