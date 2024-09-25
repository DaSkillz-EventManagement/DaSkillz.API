using Domain.DTOs.Coupons;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Coupons.Command.CreateCoupon
{
    public class CreateCouponCommand : IRequest<APIResponse>
    {
        public CouponDto Coupon { get; set; }
        public CouponEventDto CouponEventDto { get; set; }

        public CreateCouponCommand()
        {

        }

        public CreateCouponCommand(CouponDto coupon, CouponEventDto couponEventDto)
        {
            Coupon = coupon;
            CouponEventDto = couponEventDto;
        }
    }
}
