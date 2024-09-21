using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Subscription?> GetByUserId(Guid? userId)
        {
            return await _context.Subscription.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<int> UpdateExpiredSubscription()
        {
            return await _context.Subscription
                .Where(s => s.IsActive && s.EndDate <= DateTime.UtcNow)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsActive, false));


        }
    }
}
