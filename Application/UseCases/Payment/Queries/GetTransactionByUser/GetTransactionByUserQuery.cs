using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetTransactionByUser
{
    public class GetTransactionByUserQuery : IRequest<APIResponse>
    {
        public Guid userId { get; set; }

        public GetTransactionByUserQuery(Guid userId)
        {
            this.userId = userId;
        }
    }
}
