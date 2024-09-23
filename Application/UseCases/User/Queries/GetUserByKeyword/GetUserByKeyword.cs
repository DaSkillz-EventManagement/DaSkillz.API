using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.GetUserByKeyword
{
    public class GetUserByKeyword : IRequest<APIResponse>
    {
        public string? keyword { get; set; }

        public GetUserByKeyword(string? keyword)
        {
            this.keyword = keyword;
        }
    }
}
