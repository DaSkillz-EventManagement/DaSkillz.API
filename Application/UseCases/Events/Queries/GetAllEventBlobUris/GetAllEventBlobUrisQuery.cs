using MediatR;

namespace Application.UseCases.Events.Queries.GetAllEventBlobUris
{
    public class GetAllEventBlobUrisQuery : IRequest<Dictionary<string, List<string>>>
    {

        public Guid EventId { get; set; }

        public GetAllEventBlobUrisQuery(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
