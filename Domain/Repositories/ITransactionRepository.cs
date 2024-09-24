using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction?>> getEventTransactionAsync(Guid? eventId);
        Task<IEnumerable<Transaction?>> getTransactionByUserIdAsync(Guid? guid);
        Task<IList<Transaction>> getProcessingTransaction();
        Task<Transaction?> GetExistProcessingTransaction(Guid userId, Guid eventId);
        Task<IEnumerable<Transaction>> FilterTransactionsAsync(Guid? eventId, Guid? userId, int? status, int? subscriptionType);
    }
}
