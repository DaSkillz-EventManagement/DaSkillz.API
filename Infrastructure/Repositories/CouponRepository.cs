using Application.Helper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CouponRepository : RepositoryBase<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetListUserIdByCouponId(string CouponId)
        {
            return await _context.Coupons
                .Where(c => c.Id == CouponId)
                .SelectMany(c => c.Users)
                .ToListAsync();
        }

        public async Task<bool> ValidateCouponOnThisEvent(string CouponId, Guid EventId)
        {
           return await _context.Events
           .AnyAsync(e => e.EventId == EventId && e.Coupons.Any(c => c.ExpiredDate > DateTimeHelper.GetCurrentTimeAsLong() && c.NOAttempts > 0) && e.Coupons.Any(c => c.Id.Equals(CouponId)));
        }
    }
}
