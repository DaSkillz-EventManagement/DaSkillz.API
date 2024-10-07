using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalTransactionByDate
{
    public class GetTotalTransactionByDate : IRequest<APIResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDay { get; set; }

        public GetTotalTransactionByDate(DateTime startDate, DateTime endDate, bool isDay)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsDay = isDay;
        }

    }

}
