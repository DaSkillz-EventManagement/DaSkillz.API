using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignOut
{
    public class SignOutCommand : IRequest<APIResponse>
    {
        public string userId { get; set; }

        public SignOutCommand(string userId)
        {
            this.userId = userId;
        }
    }
}
