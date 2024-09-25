using Domain.Enum.Price;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Prices.Queries.GetAll;

public class GetAllPriceQuery : IRequest<APIResponse>
{
    public GetAllPriceOrderBy OrderBy { get; set; }
    public bool IsAscending { get; set; }
    public GetAllPriceQuery(GetAllPriceOrderBy orderBy, bool isAscending)
    {
        OrderBy = orderBy;
        IsAscending = isAscending;
    }
}
