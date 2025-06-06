﻿using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Coupons.Queries.GetCoupon
{
    public class GetCouponQueryHandler : IRequestHandler<GetCouponQuery, PagedList<Coupon>>
    {
        private readonly ICouponRepository _couponRepository;

        public GetCouponQueryHandler(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<PagedList<Coupon>> Handle(GetCouponQuery request, CancellationToken cancellationToken)
        {
            var couponList = await _couponRepository.GetAll();
            var result = new PagedList<Coupon>()
            {
                Items = couponList,
                TotalItems = couponList.Count(),
                CurrentPage = request.PageNo,
                EachPage = request.ElementEachPage,
            };
            return result;


        }
    }
}
