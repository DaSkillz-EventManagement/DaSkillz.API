using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Coupons.Queries.GetUsersByCoupon
{
    public class GetUsersByCouponQueryHandler : IRequestHandler<GetUsersByCouponQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public async Task<APIResponse> Handle(GetUsersByCouponQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var couponEntity = _couponRepository.GetById(request.CouponId);
            if (couponEntity == null)
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
            }

            var listUser = await _couponRepository.GetListUserIdByCouponId(request.CouponId);
            var data = _mapper.Map<List<UserResponseDto>>(listUser);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.Complete;
            response.Data = data;

            return response;

        }
    }
}
