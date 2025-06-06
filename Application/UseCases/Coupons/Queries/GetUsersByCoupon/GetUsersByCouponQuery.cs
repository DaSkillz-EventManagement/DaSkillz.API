﻿using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Coupons.Queries.GetUsersByCoupon
{
    public class GetUsersByCouponQuery : IRequest<APIResponse>
    {
        public string CouponId { get; set; }

        public GetUsersByCouponQuery()
        {
        }

        public GetUsersByCouponQuery(string couponId)
        {
            CouponId = couponId;

        }
    }
}
