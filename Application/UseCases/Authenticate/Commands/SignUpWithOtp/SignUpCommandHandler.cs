using Application.Abstractions.AvatarApi;
using Application.Helper;
using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Commands.SignUpWithOtp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAvatarApiClient _avatarApiClient;

        public SignUpCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IAvatarApiClient avatarApiClient)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _avatarApiClient = avatarApiClient;
        }

        public async Task<APIResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
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

            var avatarApiClient = _avatarApiClient.GetAvatarUrlWithName(request.FullName!);
            var id = Guid.NewGuid();
            user = new Domain.Entities.User
            {
                UserId = id,
                Email = request.Email,
                FullName = request.FullName,
                Status = AccountStatus.Pending.ToString(),
                Avatar = avatarApiClient,
                RoleId = Convert.ToInt32(UserRole.User),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            await _userRepository.Add(user);

            var otp = AuthenHelper.GenerateOTP();
            //var existingUserValidation = await _unitOfWork.UserValidationRepository.GetUser(userId);

            //bool isNewUserValidation = existingUserValidation == null;
            //var userValidation = existingUserValidation ?? new UserValidation { UserId = userId };

            //userValidation.Otp = otp;
            //userValidation.ExpiredAt = DateTime.UtcNow.AddMinutes(5);
            //userValidation.CreatedAt = userValidation.CreatedAt == default ? DateTime.UtcNow : userValidation.CreatedAt;

            //if (isNewUserValidation)
            //{
            //    await _unitOfWork.UserValidationRepository.Add(userValidation);
            //}
            //else
            //{
            //    await _unitOfWork.UserValidationRepository.Update(userValidation);
            //}

            //await _unitOfWork.SaveChangesAsync();

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
