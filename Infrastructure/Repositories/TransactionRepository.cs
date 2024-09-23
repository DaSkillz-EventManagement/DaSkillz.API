using Domain.Entities;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch.Security;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Transaction?> getTransactionByUserIdAsync(Guid? guid)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.UserId.Equals(guid));
        }        
        
        public async Task<Transaction?> getEventTransactionAsync(Guid? eventId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.EventId.Equals(eventId));
        }

        public async Task<Transaction?> GetLatestTransactionIsSubscribe(Guid userId)
        {
            return await _context.Transactions
                    .Where(t => t.UserId == userId && t.IsSubscription && t.Status == 1)
                    .OrderByDescending(t => t.CreatedAt) 
                    .FirstOrDefaultAsync();
        }

        public async Task<IList<Transaction>> getProcessingTransaction()
        {
            return await _context.Transactions
                    .Where(t => t.Status == 3)
                    .OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

    }
}
