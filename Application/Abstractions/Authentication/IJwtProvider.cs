using Domain.DTOs.AuthenticationDTO;
using Domain.Models.Response;

namespace Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<string> GenerateAccessToken(string email);
        string GenerateRefreshToken();
        Task<APIResponse> RefreshToken(TokenResponseDTO token);
    }
}
