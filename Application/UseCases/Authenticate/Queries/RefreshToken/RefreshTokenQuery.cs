using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<APIResponse>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public RefreshTokenQuery(string? accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
