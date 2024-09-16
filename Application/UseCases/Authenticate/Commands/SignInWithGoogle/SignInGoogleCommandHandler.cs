using Application.Abstractions.Authentication;
using Application.Abstractions.Oauth2;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle
{
    public class SignInGoogleCommandHandler : IRequestHandler<SignInGoogleCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGoogleTokenValidation _googleTokenValidation;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public SignInGoogleCommandHandler(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IGoogleTokenValidation googleTokenValidation,
            IMapper mapper,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _googleTokenValidation = googleTokenValidation;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
        }

        public async Task<APIResponse> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
        {
            var tokenValidationResponse = await _googleTokenValidation.ValidateGoogleTokenAsync(request.Token!);
            if (tokenValidationResponse.StatusResponse != HttpStatusCode.OK)
            {
                return tokenValidationResponse;
            }

            //var email = tokenValidationResponse.Data!.ToString();

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                var newUser = new Domain.Entities.User
                {
                    UserId = Guid.NewGuid(),
                    Email = request.Email,
                    FullName = request.FullName,
                    Status = AccountStatus.Active.ToString(),
                    Avatar = request.PhotoUrl,
                    RoleId = Convert.ToInt32(UserRole.User),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                await _userRepository.AddUser(newUser);

                var response = await _jwtProvider.GenerateAccessRefreshTokens(newUser.UserId, newUser.Email);

                await _unitOfWork.SaveChangesAsync();

                var data = _mapper.Map<UserResponseDto>(newUser);
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageUser.RegisterSuccess,
                    Data = new LoginResponseDto
                    {
                        UserData = data,
                        Token = response
                    }
                };

            };

            var a = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (a == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageUser.UserNotFound,
                    Data = null
                };
            }

            //check if the refresh token exists, then remove it to create new refresh token ahihi
            var existingRefreshTokens = await _refreshTokenRepository.GetUserByIdAsync(user.UserId);
            if (existingRefreshTokens != null)
            {
                await _refreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshTokens.Token);
            }
            //create new refresh token
            var tokenResponse = await _jwtProvider.GenerateAccessRefreshTokens(user.UserId, user.Email!);
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.LoginSuccess,
                Data = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = tokenResponse
                }
            };
        }
    }
}
