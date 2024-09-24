using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Prices.Queries.GetById;

public class GetPriceByIdQuery : IRequest<APIResponse>
{
    public int PriceId { get; set; }
    public GetPriceByIdQuery(int id)
    {
        PriceId = id;
    }
}
