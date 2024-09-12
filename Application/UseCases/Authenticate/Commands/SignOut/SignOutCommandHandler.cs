using Application.Abstractions.AvatarApi;
using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Commands.SignOut
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvatarApiClient _avatarApiClient;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignOutCommandHandler(IUserRepository userRepository,
            IAvatarApiClient avatarApiClient,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _avatarApiClient = avatarApiClient;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            var tokenEntity = await _refreshTokenRepository.GetUserByIdAsync(Guid.Parse(request.userId));
            if (tokenEntity == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.InvalidToken,
                    Data = null
                };
            }

            await _refreshTokenRepository.RemoveRefreshTokenAsync(tokenEntity.Token);

            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.LogoutSuccess,
                Data = null
            };
        }
    }
}
