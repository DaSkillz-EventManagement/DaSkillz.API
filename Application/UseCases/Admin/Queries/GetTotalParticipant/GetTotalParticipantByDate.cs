using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalParticipant
{
    public class GetTotalParticipantByDateQuery : IRequest<APIResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDay { get; set; } 

        public GetTotalParticipantByDateQuery(DateTime startDate, DateTime endDate, bool isDay)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsDay = isDay;
        }

    }

}
