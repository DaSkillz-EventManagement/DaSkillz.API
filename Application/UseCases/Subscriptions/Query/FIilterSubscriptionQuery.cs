using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Subscriptions.Query
{
    public class FIilterSubscriptionQuery : IRequest<APIResponse>
    {
        public Guid? userId { get; set; }
        public bool? isActive { get; set; }

        public FIilterSubscriptionQuery(Guid? userId, bool? isActive)
        {
            this.userId = userId;
            this.isActive = isActive;
        }
    }
}
