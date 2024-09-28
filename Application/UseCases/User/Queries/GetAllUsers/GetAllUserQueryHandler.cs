using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.GetAllUsers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _caching;

        public GetAllUserQueryHandler(IUserRepository userRepository, IMapper mapper, IRedisCaching caching)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _caching = caching;
        }



        public async Task<APIResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
            var cacheKey = "all_user";
            var cachingData = await _caching.GetAsync<IEnumerable<Domain.Entities.User>>(cacheKey);
            if (cachingData != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = cachingData
                };
            }

            var users = await _userRepository.GetAllUser(request.Page, request.Pagesize, "Email");
            var usersResponse = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            await _caching.SetAsync(cacheKey, usersResponse, 15);


            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = usersResponse ?? null
            };
        }
    }
}
