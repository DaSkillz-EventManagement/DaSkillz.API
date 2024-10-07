using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalTransactionByDate
{
    public class GetTotalTransactionByDate : IRequest<APIResponse>
    {
        public Guid userId { get; set; }
        public Guid? eventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDay { get; set; }
        public int? TransactionType { get; set; }

        public GetTotalTransactionByDate(Guid userId, Guid? eventId, DateTime startDate, DateTime endDate, bool isDay, int? transactionType)
        {
            this.userId = userId;
            this.eventId = eventId;
            StartDate = startDate;
            EndDate = endDate;
            IsDay = isDay;
            TransactionType = transactionType;
        }
    }

}
