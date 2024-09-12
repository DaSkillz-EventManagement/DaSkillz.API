using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignUpWithOtp
{
    public class SignUpCommand : IRequest<APIResponse>
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }

        public SignUpCommand(string? email, string? fullName)
        {
            Email = email;
            FullName = fullName;
        }
    }
}
