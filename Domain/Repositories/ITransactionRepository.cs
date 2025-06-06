﻿using Domain.DTOs.Payment.Response;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<DailyTransaction>> GetTotalAmountByDayAsync(Guid? userId, Guid? eventId, DateTime startDate, DateTime endDate, int? type);
        Task<List<HourlyTransaction>> GetTotalAmountByHourAsync(Guid? userId, Guid? eventId, DateTime startDate, DateTime endDate, int? type);
        Task<IEnumerable<Transaction?>> getEventTransactionAsync(Guid? eventId);
        Task<IEnumerable<Transaction?>> getTransactionByUserIdAsync(Guid? guid);
        Task<IList<Transaction>> getProcessingTransaction();
        Task<Transaction?> GetExistProcessingTransaction(Guid userId, Guid eventId);
        Task<IEnumerable<Transaction>> FilterTransactionsAsync(Guid? eventId, Guid? userId, int? status, int? subscriptionType);
        Task<Transaction?> GetAlreadyPaid(Guid userId, Guid eventId);

        Task<List<AccountStatisticDto>> GetAccountStatistics();
        Task<List<AccountStatisticInfoDto>> GetAccountStatisticInfoByEventId(Guid userId);
        Task<TicketStatisticDto> GetTicketStatistic();
    }
}
