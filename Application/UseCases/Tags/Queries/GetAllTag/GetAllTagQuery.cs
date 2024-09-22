using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Tags.Queries.GetAllTag
{
    public class GetAllTagQuery : IRequest<APIResponse>
    {
        public int page { get; set; }
        public int eachPage { get; set; }

        public GetAllTagQuery(int page, int eachPage)
        {
            this.page = page;
            this.eachPage = eachPage;
        }
    }
}
