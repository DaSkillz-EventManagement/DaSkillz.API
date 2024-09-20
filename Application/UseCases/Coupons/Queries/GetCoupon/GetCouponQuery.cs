using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Queries.GetCoupon
{
    public class GetCouponQuery : IRequest<PagedList<Coupon>>
    {
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetCouponQuery()
        {
            PageNo = 1;
            ElementEachPage = 10;
        }

        public GetCouponQuery(int pageNo, int elementEachPage)
        {
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
