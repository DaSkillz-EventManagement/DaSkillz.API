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

        public DeleteCouponCommand(int id)
        {
            Id = id;
        }
    }
}
