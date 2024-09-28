using Application.Abstractions.Caching;
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
        private readonly IRedisCaching _caching;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper, IRedisCaching caching)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
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



            //Xóa tất cả cache mà có từ khóa nằm trong cacheKey
            var invalidateCache = await _caching.SearchKeysAsync("user");
            if (invalidateCache != null)
            {
                foreach (var key in invalidateCache)
                    await _caching.DeleteKeyAsync(key);
            }

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = null,
            };
        }
    }
}
