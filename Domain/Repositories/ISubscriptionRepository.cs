using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<Subscription?> GetByUserId(Guid? userId);
        Task<int> UpdateExpiredSubscription();
    }
}
