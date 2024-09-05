using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
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

        public GetAllUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUser(request.Page, request.Pagesize, "Email");
            var usersResponse = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = usersResponse
            };
        }
    }
}
