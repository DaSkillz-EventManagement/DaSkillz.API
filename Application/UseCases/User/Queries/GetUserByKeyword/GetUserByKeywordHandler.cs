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

        public GetUserByKeywordHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<APIResponse> Handle(GetUserByKeyword request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersByKeywordAsync(request.keyword);
            var usersResponse = _mapper.Map<IEnumerable<UserByKeywordResponseDto>>(users);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = usersResponse
            };
        }
    }
}
