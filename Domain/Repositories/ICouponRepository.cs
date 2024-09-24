using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Task<bool> ValidateCouponOnThisEvent(string CouponId, Guid EventId);
        Task<List<User>> GetListUserIdByCouponId(string CouponId);
    }
}
