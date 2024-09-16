using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class RefundTransactionRepository : RepositoryBase<RefundTransaction>, IRefundTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public RefundTransactionRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
