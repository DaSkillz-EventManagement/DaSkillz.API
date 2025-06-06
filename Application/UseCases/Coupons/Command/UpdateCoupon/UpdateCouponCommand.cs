﻿using Domain.DTOs.Coupons;
using Domain.Entities;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Coupons.Command.UpdateCoupon
{
    public class UpdateCouponCommand : IRequest<APIResponse>
    {
        public Coupon Coupon { get; set; }
        public CouponEventDto CouponEventDto { get; set; }

        public UpdateCouponCommand()
        {
        }

        public UpdateCouponCommand(Coupon coupon, CouponEventDto couponEventDto)
        {
            Coupon = coupon;
            CouponEventDto = couponEventDto;
        }
    }
}
