using Application.Abstractions.Authentication;
using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Queries.ValidateOTP
{
    public class ValidateOtpQueryHandler : IRequestHandler<ValidateOtpQuery, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _redisCaching;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public ValidateOtpQueryHandler(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRedisCaching redisCaching,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _redisCaching = redisCaching;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(ValidateOtpQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageUser.UserNotFound,
                    Data = null
                };
            }

            var existOTP = await _redisCaching.GetAsync<UserValidation>($"SignIn_{request.Email}");
            if (existOTP.Otp == request.Otp)
            {

                var tokenResponse = await _jwtProvider.GenerateAccessRefreshTokens(user.UserId, user.Email!);

                if (user.Status == AccountStatus.Pending.ToString())
                {
                    user.Status = AccountStatus.Active.ToString();
                }

                await _redisCaching.RemoveAsync($"SignIn_{request.Email}");
                await _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                var userDTO = _mapper.Map<UserResponseDto>(user);
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageUser.ValidateSuccessfully,
                    Data = new LoginResponseDto
                    {
                        UserData = userDTO,
                        Token = tokenResponse
                    }
                };
            }

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageUser.ValidateFailed,
                Data = null
            };
        }
    }
}
