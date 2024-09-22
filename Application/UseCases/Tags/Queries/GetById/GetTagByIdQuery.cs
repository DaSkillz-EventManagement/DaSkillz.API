using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Tags.Queries.GetById
{
    public class GetTagByIdQuery : IRequest<APIResponse>
    {
        public int tagId { get; set; }

    }
}
