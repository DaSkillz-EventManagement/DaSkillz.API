using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetAllTransaction
{
    public class GetAllTransactionQuery : IRequest<APIResponse>
    {
    }
}
