using Application.Abstractions.Authentication;
using Application.ResponseMessage;
using Domain.Constants.Authenticate;
using Domain.DTOs.AuthenticationDTO;
using Domain.Models.Response;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch.Core.TermVectors;
using Event_Management.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.UseCases.Authenticate.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, APIResponse>
    {

        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;

        public RefreshTokenQueryHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<APIResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            //validate the token if it's a jwt token or not, then extract information to create new token
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);

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
            var existingRefreshToken = await _refreshTokenRepository.GetTokenAsync(request.RefreshToken!);
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

            var newAccessToken = await _jwtProvider.GenerateAccessToken(emailClaim!.Value);

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageUser.TokenRefreshSuccess,
                Data = new
                {
                    AccessToken = newAccessToken
                }
            };
        }
    }
}
