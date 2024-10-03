using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.CheckUserPremium
{
    public class CheckUserPremiumHandler : IRequestHandler<CheckUserPremiumQuery, APIResponse>
    {
        //private readonly IUs
        public Task<APIResponse> Handle(CheckUserPremiumQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
