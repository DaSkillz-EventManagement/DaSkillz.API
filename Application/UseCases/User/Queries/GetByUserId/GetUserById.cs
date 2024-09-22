using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.GetByUserId
{
    public class GetUserById : IRequest<APIResponse>
    {
        public Guid userId { get; set; }

        public GetUserById(Guid userId)
        {
            this.userId = userId;
        }
    }
}
