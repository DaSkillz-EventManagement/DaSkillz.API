﻿using Domain.DTOs.Coupons;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Coupons.Command.UseCoupon
{
    public class UseCouponCommand : IRequest<APIResponse>
    {
        public string CouponId { get; set; }
        public CouponEventDto CouponEventDto { get; set; }


        public UseCouponCommand()
        {
        }

        public UseCouponCommand(string couponId, CouponEventDto couponEventDto)
        {
            CouponId = couponId;
            CouponEventDto = couponEventDto;
        }
    }
}
