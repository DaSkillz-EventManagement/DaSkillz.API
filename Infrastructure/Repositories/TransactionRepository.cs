using Application.Helper;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.User.Response;
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

        public async Task<List<DailyTransaction>> GetTotalAmountByDayAsync(Guid? eventId, DateTime startDate, DateTime endDate)
        {
            DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);
            var transactions = await _context.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDateAdjusted)
                .ToListAsync();

            if (eventId != null)
            {
                transactions = transactions.Where(p => p.EventId == eventId.Value).ToList();
            }

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

        public async Task<List<HourlyTransaction>> GetTotalAmountByHourAsync(Guid? eventId, DateTime startDate, DateTime endDate)
        {
            DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);

            var transactions = await _context.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDateAdjusted)
                .ToListAsync();

            if (eventId != null)
            {
                transactions = transactions.Where(p => p.EventId == eventId.Value).ToList();
            }

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

        public async Task<List<AccountStatisticDto>> GetAccountStatistics()
        {
            return await Task.Run(() => _context.Transactions
                .Where(t => t.SubscriptionType == (int)PaymentType.SUBSCRIPTION)
                .AsEnumerable()
                .GroupBy(t => t.UserId)
                .Select(group => new AccountStatisticDto
                {
                    UserId = (Guid)group.Key,
                    NumOfPurchasing = group.Count(),
                    TotalMoney = group.Sum(t => decimal.TryParse(t.Amount, out var amount) ? amount : 0) // Handle parsing
                })
                .ToList());
        }

        public async Task<List<AccountStatisticInfoDto>> GetAccountStatisticInfoByEventId(Guid userId)
        {
            return await Task.Run(() => _context.Transactions
                .Where(t => t.UserId.Equals(userId) && t.SubscriptionType == (int)PaymentType.SUBSCRIPTION) // Filter by EventId
                .AsEnumerable()
                .Select(t => new AccountStatisticInfoDto
                {
                    StartDate = t.CreatedAt,
                    EndDate = t.CreatedAt.AddMonths(1), // EndDate is 1 month after CreatedDate
                    Amount = decimal.TryParse(t.Amount, out var amount) ? amount : 0, // Parse the Amount
                    IsActive = t.CreatedAt.AddMonths(1) >= DateTime.Now // If EndDate >= Now, IsActive is true (1), otherwise false (0)
                })
                .ToList());
        }
    }
}
