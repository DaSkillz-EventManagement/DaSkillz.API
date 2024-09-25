using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Coupons.Command.UseCoupon
{
    public class UseCouponCommandHandler : IRequestHandler<UseCouponCommand, APIResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public async Task<APIResponse> Handle(UseCouponCommand request, CancellationToken cancellationToken)
        {

            var response = new APIResponse();
            if (request.CouponEventDto.UserId == null)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.Unauthorized;
            }
            var validateCoupon = await _couponRepository.ValidateCouponOnThisEvent(request.CouponId, request.CouponEventDto.EventId);
            if (!validateCoupon)
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;

            }

            var couponEntity = await _couponRepository.GetById(request.CouponId);
            var eventEntity = await _eventRepository.GetById(request.CouponEventDto.EventId);
            switch (couponEntity.DiscountType)
            {
                case "Money":
                    eventEntity.Fare -= couponEntity.Value;
                    break;
                case "Voucher":
                    eventEntity.Fare = (1 - (couponEntity.Value) * 0.01m) * eventEntity.Fare;
                    break;


            }
            var eventResponse = _mapper.Map<EventResponseDto>(eventEntity);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.SavingSuccesfully;

            return response;

        }
    }
}
