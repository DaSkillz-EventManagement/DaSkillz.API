using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetTransactionByEvent
{
    public class GetEventTransactionQuery : IRequest<APIResponse>
    {
        public Guid eventId {  get; set; }

        public GetEventTransactionQuery(Guid eventId)
        {
            this.eventId = eventId;
        }
    }
}
