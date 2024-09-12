using Application.Abstractions.AvatarApi;
using Application.Abstractions.Caching;
using Application.Abstractions.Email;
using Application.Helper;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Commands.SignInWIthOtp
{
    public class SignInOtpCommandHandler : IRequestHandler<SignInOtpCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _redisCaching;
        private readonly IAvatarApiClient _avatarApiClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SignInOtpCommandHandler(IUserRepository userRepository, IRedisCaching redisCaching, IAvatarApiClient avatarApiClient, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userRepository = userRepository;
            _redisCaching = redisCaching;
            _avatarApiClient = avatarApiClient;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<APIResponse> Handle(SignInOtpCommand request, CancellationToken cancellationToken)
        {
            string RedisDbkey = $"SignIn_{request.Email}";
            var user = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User not existed",
                    Data = null
                };
            }

            var otp = AuthenHelper.GenerateOTP();

            var existingUserValidation = await _redisCaching.GetAsync<UserValidation>(RedisDbkey);
            var userValidation = new UserValidation
            {
                UserId = user.UserId,
                Otp = otp
            };

            await _redisCaching.SetAsync(RedisDbkey, userValidation, 5);
            await _emailService.SendEmailAsync(request.Email, user.FullName, otp, "./template/VerifyWithOTP.cshtml");

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.OTPSuccess,
                Data = null
            };
        }
    }
}
