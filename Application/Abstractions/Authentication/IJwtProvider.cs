using Domain.DTOs.AuthenticationDTO;
using Domain.Models.Response;

namespace Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        /// <summary>
        /// generate access token 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GenerateAccessToken(string email);

        /// <summary>
        /// refresh exist token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<APIResponse> RefreshToken(TokenResponseDTO token);

        /// <summary>
        /// Generate both access token and refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<TokenResponseDTO> GenerateAccessRefreshTokens(Guid userId, string email);
    }
}
