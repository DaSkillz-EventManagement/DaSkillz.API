using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.GetByUserId
{
    public class GetUserByIdHandler : IRequestHandler<GetUserById, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<APIResponse> Handle(GetUserById request, CancellationToken cancellationToken)
        {
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
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = mapUser,
            };
        }
    }
}
