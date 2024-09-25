using Domain.DTOs.Coupons;
using MediatR;

namespace Application.UseCases.Coupons.Command.DeleteCoupon
{
    public class DeleteCouponCommand : IRequest<bool>
    {
        public int Id;
        public CouponEventDto CouponEventDto { get; set; }

        public DeleteCouponCommand()
        {
        }

        public DeleteCouponCommand(int id, CouponEventDto couponEventDto)
        {
            Id = id;
            CouponEventDto = couponEventDto;
        }
    }
}
