using Application.ResponseMessage;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Globalization;

namespace Application.UseCases.Admin.Queries.GetTotalTransactionByDate
{
    public class GetTotalTransactionByDateHandler : IRequestHandler<GetTotalTransactionByDate, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTotalTransactionByDateHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetTotalTransactionByDate request, CancellationToken cancellationToken)
        {
            var transactionTypes = new Dictionary<int, string>
            {
                { 1, "Ticket" },
                { 2, "Sponsor" },
                { 3, "Advertise" },
                { 4, "Subscription" }
            };

            var totalsByType = new Dictionary<string, decimal>
                {
                    { "Ticket", 0m },   // Ticket
                    { "Sponsor", 0m },  // Sponsor
                    { "Advertise", 0m },// Advertise
                    { "Subscription", 0m } // Subscription
                };
            List<DailyTransaction> transactionsByDay;
            if (request.TransactionType == null)
            {
                transactionsByDay = await _transactionRepository.GetTotalAmountByDayAsync(request.userId, request.eventId, request.StartDate, request.EndDate, null);
            }
            else
            {
                transactionsByDay = await _transactionRepository.GetTotalAmountByDayAsync(request.userId, request.eventId, request.StartDate, request.EndDate, request.TransactionType);

            }
            if (request.IsDay)
            {
                var dailyTransactions = new List<DailyTransaction>(); 
                decimal totalAmountDay = 0; 

                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    var dayString = day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dailyTotalsByType = new Dictionary<string, decimal>(totalsByType);

                    var transactionsForDay = transactionsByDay.Where(t => t.Date == dayString).ToList();

                    decimal totalAmountForDay = 0;

                    // Update daily totals by type
                    foreach (var transaction in transactionsForDay)
                    {
                        if (transactionTypes.ContainsKey(transaction.SubscriptionType))
                        {
                            var type = transactionTypes[transaction.SubscriptionType];
                            var amount = decimal.Parse(transaction.TotalAmount);
                            dailyTotalsByType[type] += amount; // Add to the specific type for the day
                            totalAmountForDay += amount; // Add to the daily total
                        }
                    }

                    totalAmountDay += totalAmountForDay; // Add the day's total to the overall total

                    // Add daily transaction breakdown to the result
                    dailyTransactions.Add(new DailyTransaction
                    {
                        Date = dayString,
                        TotalAmount = totalAmountForDay.ToString(),
                        TotalsByType = dailyTotalsByType.ToDictionary(k => k.Key, v => v.Value.ToString()) 
                    });
                }


                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.OK,
                    Message = MessageCommon.GetSuccesfully,
                    Data = new
                    {
                        EventId = request.eventId,
                        TotalAmount = totalAmountDay.ToString(),
                        TotalsByType = totalsByType.ToDictionary(k => k.Key, v => v.Value.ToString()),
                        DailyTransactions = dailyTransactions
                    }
                };
            }
            else
            {
                var transactionsByHour = await _transactionRepository.GetTotalAmountByHourAsync(request.userId, request.eventId, request.StartDate, request.EndDate, request.TransactionType);
                var hourlyTransactions = new List<HourlyTransaction>(); 
                decimal totalAmountHour = 0;

                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    int startHour = (day.Date == request.StartDate.Date) ? request.StartDate.Hour : 0; 
                    int endHour = (day.Date == request.EndDate.Date) ? request.EndDate.Hour : 23; 

                    for (var hour = startHour; hour <= endHour; hour++) 
                    {
                        var hourString = $"{hour:D2}:00"; 
                        var transactionsForHour = transactionsByHour.Where(t => t.Hour == hourString).ToList();

                        var hourlyTotalsByType = new Dictionary<string, decimal>(totalsByType);

                        decimal totalAmountForHour = 0;

                        foreach (var transaction in transactionsForHour)
                        {
                            if (transactionTypes.ContainsKey(transaction.SubscriptionType))
                            {
                                var type = transactionTypes[transaction.SubscriptionType];
                                var amount = decimal.Parse(transaction.TotalAmount);
                                hourlyTotalsByType[type] += amount; 
                                totalAmountForHour += amount; 
                            }
                        }

                        totalAmountHour += totalAmountForHour; 

                        hourlyTransactions.Add(new HourlyTransaction
                        {
                            Date = day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Hour = hourString,
                            TotalAmount = totalAmountForHour.ToString(),
                            TotalsByType = hourlyTotalsByType.ToDictionary(k => k.Key, v => v.Value.ToString())
                        });
                    }
                }

                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.OK,
                    Message = MessageCommon.GetSuccesfully,
                    Data = new
                    {
                        EventId = request.eventId,
                        TotalAmount = totalAmountHour.ToString(),
                        HourlyTransactions = hourlyTransactions
                    }
                };
            }

        }
    }
}
