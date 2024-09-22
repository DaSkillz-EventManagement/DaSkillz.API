using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Tags.Queries.SearchTag
{
    public class SearchTagQuery : IRequest<APIResponse>
    {
        public string? searchTag { get; set; }
    }
}
