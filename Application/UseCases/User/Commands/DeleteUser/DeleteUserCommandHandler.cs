using Application.ResponseMessage;
using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existUsers = await _userRepository.GetUserByIdAsync(request.Id);
            if (existUsers == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageUser.UserNotFound,
                    Data = null,
                };
            }

            if (existUsers.RoleId == (int)UserRole.Admin)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Forbidden,
                    Message = "Can't block admin",
                    Data = null,
                };
            }

            if (existUsers.Status == AccountStatus.Active.ToString())
            {
                existUsers.Status = AccountStatus.Blocked.ToString();
            }
            else if (existUsers.Status == AccountStatus.Blocked.ToString())
            {
                existUsers.Status = AccountStatus.Active.ToString();
            }
            existUsers.UpdatedAt = DateTime.UtcNow;
            await _userRepository.Update(existUsers);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = null,
            };
        }
    }
}
