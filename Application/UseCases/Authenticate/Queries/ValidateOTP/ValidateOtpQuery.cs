using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Queries.ValidateOTP
{
    public class ValidateOtpQuery : IRequest<APIResponse>
    {
        public string? Email { get; set; }
        public string? Otp { get; set; }

        public ValidateOtpQuery(string? email, string? otp)
        {
            Email = email;
            Otp = otp;
        }
    }
}
