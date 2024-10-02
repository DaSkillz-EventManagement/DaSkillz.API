using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.EventStatistic.Queries.GetEventStatisticsById
{
    public class GetEventStatisticsByIdQuery : IRequest<APIResponse>
    {
        public Guid EventId {  get; set; }
        public Guid UserId {  get; set; }

        public GetEventStatisticsByIdQuery(Guid eventId, Guid userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}
