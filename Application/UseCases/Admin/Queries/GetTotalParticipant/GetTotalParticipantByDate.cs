using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalParticipant
{
    public class GetTotalParticipantByDateQuery : IRequest<APIResponse>
    {
        public Guid? eventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDay { get; set; }

        public GetTotalParticipantByDateQuery(Guid? eventId, DateTime startDate, DateTime endDate, bool isDay)
        {
            this.eventId = eventId;
            StartDate = startDate;
            EndDate = endDate;
            IsDay = isDay;
        }
    }

}
