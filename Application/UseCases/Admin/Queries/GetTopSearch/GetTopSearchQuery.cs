using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTopSearch
{
    public class GetTopSearchQuery : IRequest<APIResponse>
    {
        public int size { get; set; }

        public GetTopSearchQuery(int size)
        {
            this.size = size;
        }
    }
}
