using Application.Abstractions.Caching;
using Application.ExternalServices.Images;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _caching;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper, IRedisCaching caching)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
            var existUsers = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (existUsers == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageUser.UserNotFound,
                    Data = null,
                };
            }


            existUsers.Phone = request.Phone;

            existUsers.FullName = request.FullName;

            bool isBase64 = Utilities.IsBase64String(request.Avatar!);
            if (!string.IsNullOrWhiteSpace(request.Avatar) && isBase64)
            {
                string url = existUsers.Avatar!;
                int startIndex = url.LastIndexOf("/eventcontainer/") + "/eventcontainer/".Length;
                string result = url.Substring(startIndex);
                await _imageService.DeleteBlob(result);
                existUsers.Avatar = await _imageService.UploadImage(request.Avatar, Guid.NewGuid());
            }

            await _userRepository.Update(existUsers);
            await _unitOfWork.SaveChangesAsync();
            var updatedUsers = _mapper.Map<UserUpdatedResponseDto>(existUsers);


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
                Message = MessageCommon.UpdateSuccesfully,
                Data = updatedUsers,
            };
        }
    }
}
