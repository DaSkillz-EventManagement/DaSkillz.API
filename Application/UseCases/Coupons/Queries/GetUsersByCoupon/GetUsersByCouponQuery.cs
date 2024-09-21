using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Queries.GetUsersByCoupon
{
    public class GetUsersByCouponQuery : IRequest<APIResponse>
    {
        public string CouponId {  get; set; }
        
        public GetUsersByCouponQuery()
        {
        }

        public GetUsersByCouponQuery(string couponId)
        {
            CouponId = couponId;
           
        }
    }
}
