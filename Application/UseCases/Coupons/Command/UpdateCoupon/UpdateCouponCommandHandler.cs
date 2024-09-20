using Application.UseCases.Coupons.Command.CreateCoupon;
using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseMessage;
using System.Net;

namespace Application.UseCases.Coupons.Command.UpdateCoupon
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, APIResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepository _eventRepository;

        public UpdateCouponCommandHandler(ICouponRepository couponRepository, IMapper mapper, IUnitOfWork unitOfWork, IEventRepository eventRepository)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _eventRepository = eventRepository;
        }

        public async Task<APIResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepository.IsOwner(request.CouponEventDto.UserId, request.CouponEventDto.EventId);
            if (isOwner)
            {
                var couponEntity = await _couponRepository.GetById(request.Coupon.Id);
                if (couponEntity == null)
                {
                    return new APIResponse()
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound,
                        Data = null,
                    };
                }

                _mapper.Map(request.Coupon, couponEntity);


                _couponRepository.Update(couponEntity);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new APIResponse()
                    {
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.UpdateSuccesfully,
                        Data = couponEntity,
                    };
                }
                else
                {
                    return new APIResponse()
                    {
                        StatusResponse = HttpStatusCode.BadRequest,
                        Message = MessageCommon.UpdateFailed,
                        Data = request.Coupon,
                    };
                }
            }
            else
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageEvent.OnlyHostCanUpdateEvent,
                    Data = null,
                };
            }

        }
    }
}
