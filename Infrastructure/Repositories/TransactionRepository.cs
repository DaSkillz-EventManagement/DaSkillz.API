using Application.Helper;
using Domain.DTOs.Payment.Response;
using Domain.Entities;
using Domain.Enum.Payment;
using Domain.Repositories;
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

        public async Task<List<DailyTransaction>> GetTotalAmountByDayAsync(DateTime startDate, DateTime endDate)
        {
            DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);
            var transactions = await _context.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDateAdjusted)
                .ToListAsync();

            var transactionsByDay = transactions
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new DailyTransaction
                {
                    Date = g.Key.ToString("dd/MM/yyyy"),
                    TotalAmount = g.Sum(t => Utilities.ParseAmount(t.Amount)).ToString(),
                })
                .OrderBy(d => d.Date)
                .ToList();

            return transactionsByDay;
        }

        public async Task<List<HourlyTransaction>> GetTotalAmountByHourAsync(DateTime startDate, DateTime endDate)
        {
            DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);

            var transactions = await _context.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDateAdjusted)
                .ToListAsync(); 

            var transactionsByHour = transactions
                .GroupBy(t => new { Day = t.CreatedAt.Date, Hour = t.CreatedAt.Hour })
                .Select(g => new HourlyTransaction
                {
                    Date = g.Key.Day.ToString("dd/MM/yyyy"),
                    Hour = $"{g.Key.Hour:D2}:00",
                    TotalAmount = g.Sum(t => Utilities.ParseAmount(t.Amount)).ToString(), // Giữ nguyên phương thức
                })
                .OrderBy(h => h.Date)
                .ThenBy(h => h.Hour)
                .ToList();

            return transactionsByHour;
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
                    .Where(t => t.UserId == userId && t.SubscriptionType == (int)PaymentType.SUBSCRIPTION && t.Status == 1)
                    .OrderByDescending(t => t.CreatedAt) 
                    .FirstOrDefaultAsync();
        }

        public async Task<IList<Transaction>> getProcessingTransaction()
        {
            return await _context.Transactions
                    .Where(t => t.Status == 3 && t.SubscriptionType == (int)PaymentType.SUBSCRIPTION)
                    .OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<Transaction?> GetExistProcessingTransaction(Guid userId, Guid eventId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.UserId.Equals(userId)
                && t.EventId.Equals(eventId)
                && t.Status == (int)TransactionStatus.PROCESSING);
        }

        public async Task<Transaction?> GetAlreadyPaid(Guid userId, Guid eventId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.UserId.Equals(userId)
                && t.EventId.Equals(eventId)
                && t.Status == (int)TransactionStatus.SUCCESS);
        }

        public async Task<IEnumerable<Transaction>> FilterTransactionsAsync(Guid? eventId, Guid? userId, int? status, int? subscriptionType)
        {
            // Tạo query gốc
            var query = _context.Transactions.AsQueryable();

            // Lọc theo eventId
            if (eventId.HasValue)
            {
                query = query.Where(t => t.EventId == eventId);
            }

            // Lọc theo userId
            if (userId.HasValue)
            {
                query = query.Where(t => t.UserId == userId);
            }

            // Lọc theo status
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status);
            }

            // Lọc theo subscriptionType
            if (subscriptionType.HasValue)
            {
                query = query.Where(t => t.SubscriptionType == subscriptionType);
            }

            query = query.OrderByDescending(t => t.CreatedAt);

            // Thực thi query và trả về kết quả
            return await query.ToListAsync();
        }

    }
}
