using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction?> getTransactionByUserIdAsync(Guid? guid);
        Task<IList<Transaction>> getProcessingTransaction();
    }
}
