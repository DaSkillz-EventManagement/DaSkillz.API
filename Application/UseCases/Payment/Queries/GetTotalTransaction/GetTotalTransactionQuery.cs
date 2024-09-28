using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetTotalTransaction
{
    public class GetTotalTransactionQuery : IRequest<APIResponse>
    {
        public Guid? eventId { get; set; }
        public Guid? userId { get; set; }

        public GetTotalTransactionQuery(Guid? eventId, Guid? userId)
        {
            this.eventId = eventId;
            this.userId = userId;
        }
    }
}
