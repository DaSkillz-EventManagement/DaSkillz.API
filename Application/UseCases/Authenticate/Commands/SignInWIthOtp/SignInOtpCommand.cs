using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignInWIthOtp
{
    public class SignInOtpCommand : IRequest<APIResponse>
    {
        public string? Email { get; set; }

        public SignInOtpCommand(string? email)
        {
            Email = email;
        }
    }
}
