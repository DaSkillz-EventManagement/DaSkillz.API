using Domain.DTOs.Coupons;
using Domain.Entities;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Command.UpdateCoupon
{
    public class UpdateCouponCommand : IRequest<APIResponse>
    {
        public Coupon Coupon { get; set; }

        public UpdateCouponCommand(Coupon coupon)
        {
            Coupon = coupon;
        }
    }
}
