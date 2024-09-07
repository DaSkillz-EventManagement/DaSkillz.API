using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.GetTotalUserByYear
{
    public class GetTotalUserByYearQuery : IRequest<APIResponse>
    {
        public int key { get; set; }

        public GetTotalUserByYearQuery(int key)
        {
            this.key = key;
        }
    }
}
