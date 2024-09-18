using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.GetAllUsers
{
    public class GetAllUserQuery : IRequest<APIResponse>
    {
        public int Page { get; set; }
        public int Pagesize { get; set; }

        public GetAllUserQuery(int page, int pagesize)
        {
            Page = page;
            Pagesize = pagesize;
        }

    }
}
