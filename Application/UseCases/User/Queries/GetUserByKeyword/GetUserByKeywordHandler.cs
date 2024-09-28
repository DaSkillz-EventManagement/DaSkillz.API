using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.GetUserByKeyword
{
    public class GetUserByKeywordHandler : IRequestHandler<GetUserByKeyword, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _caching;

        public GetUserByKeywordHandler(IMapper mapper, IUserRepository userRepository, IRedisCaching caching)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(GetUserByKeyword request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
            string cacheKey = "all_user_keyword";
           
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

            var users = await _userRepository.GetUsersByKeywordAsync(request.keyword);
            var usersResponse = _mapper.Map<IEnumerable<UserByKeywordResponseDto>>(users);
            await _caching.SetAsync(cacheKey, usersResponse, 15);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = usersResponse
            };
        }
    }
}
