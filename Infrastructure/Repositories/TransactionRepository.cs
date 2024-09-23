using Domain.Entities;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch.Security;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction?>> getTransactionByUserIdAsync(Guid? guid)
        {
            //return await _context.Transactions
            //        .Include(t => t.Event)
            //        .Include(t => t.User)
            //        .Where(t => t.UserId == guid)
            //        .ToListAsync();
            return await _context.Transactions.Where(x => x.UserId.Equals(guid)).ToListAsync();
        }        
        
        public async Task<IEnumerable<Transaction?>> getEventTransactionAsync(Guid? eventId)
        {
            return await _context.Transactions.Where(x => x.EventId.Equals(eventId)).ToListAsync();
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
