using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Prices.Commands.Delete;

public class DeletePriceCommand : IRequest<APIResponse>
{
    public int PriceId { get; set; }
    public DeletePriceCommand(int priceId)
    {
        PriceId = priceId;
    }
}
