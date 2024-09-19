using Domain.DTOs.Coupons;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Command.CreateCoupon
{
    public class CreateCouponCommand : IRequest<APIResponse>
    {
        public CouponDto Coupon { get; set; }

        public CreateCouponCommand(CouponDto coupon)
        {
            Coupon = coupon;
        }
    }
}
