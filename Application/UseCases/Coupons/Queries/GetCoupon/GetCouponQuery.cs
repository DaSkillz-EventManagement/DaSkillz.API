using Domain.Entities;
using Domain.Models.Pagination;
using MediatR;

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
