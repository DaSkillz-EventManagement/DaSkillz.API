using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.GetByUserId
{
    public class GetUserByIdHandler : IRequestHandler<GetUserById, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _caching;

        public GetUserByIdHandler(IMapper mapper, IUserRepository userRepository, IRedisCaching caching)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
            var cacheKey = $"user_{request.userId}";
            var cachingData = await _caching.GetAsync<UserUpdatedResponseDto>(cacheKey);
            if (cachingData != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = cachingData,
                };
            }

            var user = await _userRepository.GetUserByIdAsync(request.userId);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageUser.UserNotFound,
                    Data = null,
                };
            }

            var mapUser = _mapper.Map<UserUpdatedResponseDto>(user);
            await _caching.SetAsync(cacheKey, mapUser, 240);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = mapUser,
            };
        }
    }
}
