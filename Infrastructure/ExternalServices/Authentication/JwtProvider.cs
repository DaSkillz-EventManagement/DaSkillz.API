using Application.Abstractions.Authentication;
using Application.Helper;
using Application.ResponseMessage;
using Domain.Constants.Authenticate;
using Domain.DTOs.AuthenticationDTO;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Enum;
using Infrastructure.ExternalServices.Authentication.Setting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.ExternalServices.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public JwtProvider(IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        //generate access token
        public async Task<string> GenerateAccessToken(string email)
        {

            var existUser = await _userRepository.GetUserByEmailAsync(email);
            if (existUser == null)
            {
                return "Error! Unauthorized.";
            }

            //Define information in the payload
            List<Claim> claims = new List<Claim>
            {
                new Claim(UserClaimType.UserId, existUser.UserId.ToString()),
                new Claim(ClaimTypes.Email, existUser.Email!),
                new Claim(ClaimTypes.Role, existUser.Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _jwtSettings.Securitykey ?? throw new InvalidOperationException("Secret not configured")));

            var tokenhandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.TokenExpirationInMinutes)),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(securityToken);

            return finaltoken;

        }



        //Validate the token if the token is decoded with jwt, and then extract the information in the token
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Securitykey)),
                ValidateLifetime = false //false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("HARD CODE");

            return principal;
        }


        public async Task<APIResponse> RefreshToken(TokenResponseDTO token)
        {
            //validate the token if it's a jwt token or not, then extract information to create new token
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);

            //extract userId and email from payload
            var userIdClaim = principal!.Claims.FirstOrDefault(c => c.Type == UserClaimType.UserId);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == UserClaimType.Email);

            //check user existed in the refresh token
            var existUser = await _refreshTokenRepository.GetUserByIdAsync(Guid.Parse(userIdClaim!.Value));
            var existUsers = await _userRepository.GetUserByIdAsync(Guid.Parse(userIdClaim!.Value));
            if (existUser == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = MessageUser.TokenInvalid,
                    Data = null
                };
            }
            else if (existUsers!.Status == AccountStatus.Blocked.ToString())
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = MessageCommon.Blocked,
                    Data = null
                };
            }


            //check refresh token whether it's expired or null
            var existingRefreshToken = await _refreshTokenRepository.GetTokenAsync(token.RefreshToken!);
            if (existingRefreshToken == null || existingRefreshToken.ExpireAt <= DateTime.UtcNow)
            {
                if (existingRefreshToken != null && existingRefreshToken.ExpireAt <= DateTime.UtcNow)
                {
                    await _refreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshToken.Token);
                }
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = MessageUser.TokenExpired,
                    Data = null
                };
            }




            //Using technique refresh token rotation
            //----------------------------------------------------------------

            //capture expired date from original token 
            var originalExpirationDate = existingRefreshToken.ExpireAt;

            // remove old refresh token
            await _refreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshToken.Token);

            // generate new tokens
            var newAccessToken = await GenerateAccessToken(emailClaim!.Value);
            //var newRefreshToken = AuthenHelper.GenerateRefreshToken();

            //var refreshTokenEntity = new RefreshToken
            //{
            //    UserId = existUser.UserId,
            //    Token = newRefreshToken,
            //    CreatedAt = DateTime.UtcNow,
            //    ExpireAt = originalExpirationDate
            //};

            //await _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            //----------------------------------------------------------

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageUser.TokenRefreshSuccess,
                Data = new TokenResponseDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = token.RefreshToken
                }
            };
        }

        public async Task<TokenResponseDTO> GenerateAccessRefreshTokens(Guid userId, string email)
        {
            var accessToken = await GenerateAccessToken(email);
            var refreshToken = AuthenHelper.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMonths(Convert.ToInt32(_jwtSettings.TokenExpirationInMinutes))
            };

            await _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


    }
}
