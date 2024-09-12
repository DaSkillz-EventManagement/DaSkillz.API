using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventInfo
{
    public class GetEventInfoQuery : IRequest<APIResponse>
    {
        public Guid Id { get; set; }

        public GetEventInfoQuery()
        {
        }

        public GetEventInfoQuery(Guid id)
        {
            Id = id;
        }
    }
}
