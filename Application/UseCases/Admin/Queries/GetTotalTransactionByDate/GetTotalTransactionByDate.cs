using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalTransactionByDate
{
    public class GetTotalTransactionByDate : IRequest<APIResponse>
    {
        public Guid? eventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDay { get; set; }

        public GetTotalTransactionByDate(Guid? eventId, DateTime startDate, DateTime endDate, bool isDay)
        {
            this.eventId = eventId;
            StartDate = startDate;
            EndDate = endDate;
            IsDay = isDay;
        }
    }

}
