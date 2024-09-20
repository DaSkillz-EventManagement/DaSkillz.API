using Domain.DTOs.Coupons;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
