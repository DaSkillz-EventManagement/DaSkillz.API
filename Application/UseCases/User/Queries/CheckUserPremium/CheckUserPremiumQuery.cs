using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.CheckUserPremium
{
    public class CheckUserPremiumQuery : IRequest<APIResponse>
    {
        public Guid userId { get; set; }

        public CheckUserPremiumQuery(Guid userId)
        {
            this.userId = userId;
        }
    }
}
