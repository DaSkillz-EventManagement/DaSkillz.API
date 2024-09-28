using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.FilterUser
{
    public class FilterUserQueryCommand : IRequestHandler<FilterUserQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _caching;
        private readonly IMapper _mapper;

        public FilterUserQueryCommand(IUserRepository userRepository, IRedisCaching caching, IMapper mapper)
        {
            _userRepository = userRepository;
            _caching = caching;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(FilterUserQuery request, CancellationToken cancellationToken)
        {
            //cache key phải độc nhất 
            //áp dụng chiến lược cache-aside
            //invalidate sẽ xóa cache key liên quan đến từ khóa
            //problem: nếu lượng user truy cập cao có thể dẫn đến cache stampede và cache penetration
            //solution: sẽ dụng bloom filter, clock, request coalescing (đang nghiên cứu, sẽ áp dụng sau)
            var cacheKey = $"all_filter_user_{request.UserId}_{request.FullName}_{request.Email}_{request.Phone}_{request.Status}";
            var cachingData = await _caching.GetAsync<IEnumerable<Domain.Entities.User>>(cacheKey);
            if (cachingData != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = cacheKey,
                };
            }

            var user = await _userRepository.FilterUsersAsync(
               request.UserId,
               request.FullName,
               request.Email,
               request.Phone,
               request.Status
            );

            
            var result = _mapper.Map<IEnumerable<UserResponseDto>>(user);
            await _caching.SetAsync(cacheKey, result, 15);
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }
    }
}
