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
            if (request.IsDay)
            {
                var transactionsByDay = await _transactionRepository.GetTotalAmountByDayAsync(request.userId, request.eventId, request.StartDate, request.EndDate, request.TransactionType);
                var dailyTransactions = new List<DailyTransaction>(); // Danh sách để lưu giao dịch theo ngày
                decimal totalAmountDay = 0;

                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    var dayString = day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var transactionAmount = transactionsByDay.FirstOrDefault(t => t.Date == dayString)?.TotalAmount;

                    decimal amount = decimal.TryParse(transactionAmount, out var parsedAmount) ? parsedAmount : 0;
                    totalAmountDay += amount;

                    dailyTransactions.Add(new DailyTransaction
                    {
                        Date = dayString,
                        TotalAmount = amount.ToString() // Hoặc định dạng bạn muốn
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
                        DailyTransactions = dailyTransactions
                    }
                };
            }
            else
            {
                var transactionsByHour = await _transactionRepository.GetTotalAmountByHourAsync(request.userId, request.eventId, request.StartDate, request.EndDate, request.TransactionType);
                var hourlyTransactions = new List<HourlyTransaction>(); // Danh sách để lưu giao dịch theo giờ
                decimal totalAmountHour = 0;

                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    // Xác định giờ bắt đầu và giờ kết thúc cho từng ngày
                    int startHour = (day.Date == request.StartDate.Date) ? request.StartDate.Hour : 0; // Nếu là ngày đầu tiên, bắt đầu từ giờ đã cho
                    int endHour = (day.Date == request.EndDate.Date) ? request.EndDate.Hour : 23; // Nếu là ngày cuối cùng, kết thúc tại giờ đã cho

                    for (var hour = startHour; hour <= endHour; hour++) // Duyệt qua từng giờ trong ngày
                    {
                        var hourString = $"{hour:D2}:00"; // Định dạng giờ
                        var transactionAmount = transactionsByHour.FirstOrDefault(t => t.Hour == hourString)?.TotalAmount;

                        decimal amount = decimal.TryParse(transactionAmount, out var parsedAmount) ? parsedAmount : 0;
                        totalAmountHour += amount;

                        hourlyTransactions.Add(new HourlyTransaction
                        {
                            Date = day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Hour = hourString,
                            TotalAmount = amount.ToString() // Hoặc định dạng bạn muốn
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
