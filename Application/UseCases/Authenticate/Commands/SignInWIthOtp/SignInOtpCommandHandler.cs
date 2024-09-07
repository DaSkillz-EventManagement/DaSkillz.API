using Application.Abstractions.AvatarApi;
using Application.Abstractions.Caching;
using Application.Helper;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
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

        

        public async Task<APIResponse> Handle(SignInOtpCommand request, CancellationToken cancellationToken)
        {
            string RedisDbkey = "UserValidation";
            var user = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (user != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User already exists",
                    Data = null
                };
            }

            //var avatarApiClient = _avatarApiClient.GetAvatarUrlWithName(user.FullName!);
            //var id = Guid.NewGuid();
            //user = new Domain.Entities.User
            //{
            //    UserId = id,
            //    Email = request.Email,
            //    FullName = request.FullName,
            //    Status = AccountStatus.Pending.ToString(),
            //    Avatar = avatarApiClient,
            //    RoleId = Convert.ToInt32(UserRole.User),
            //    CreatedAt = DateTime.UtcNow,
            //    UpdatedAt = null
            //};

            //await _unitOfWork.UserRepository.Add(user);
            //if (user == null)
            //{
            //    return new APIResponse
            //    {
            //        StatusResponse = HttpStatusCode.NotFound,
            //        Message = MessageUser.UserNotFound,
            //        Data = null
            //    };
            //}
            var otp = AuthenHelper.GenerateOTP();

            var existingUserValidation = await _redisCaching.GetAsync<UserValidation>(RedisDbkey);
            var userValidation = new UserValidation
            {
                UserId = user.UserId,
                Otp = otp
            };
            

            if (existingUserValidation == null)
            {
                await _redisCaching.SetAsync(RedisDbkey, userValidation, 30);
            }
            else
            {
                //await _unitOfWork.UserValidationRepository.Update(userValidation);
            }


            //_sendMailTask.SendMailVerify(new UserMailDto()
            //{
            //    UserName = userName,
            //    Email = email,
            //    OTP = otp
            //});

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.OTPSuccess,
                Data = null
            };
        }
    }
}
