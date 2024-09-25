using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetFilterTransaction
{
    public class GetFilterTransactionQuery : IRequest<APIResponse>
    {
        public Guid? eventId { get; set; }
        public Guid? userId { get; set; }
        public int? status { get; set; }
        public int? SubscriptionType { get; set; }

        public GetFilterTransactionQuery(Guid? eventId, Guid? userId, int? status, int? subscriptionType)
        {
            this.eventId = eventId;
            this.userId = userId;
            this.status = status;
            SubscriptionType = subscriptionType;
        }
    }
}
