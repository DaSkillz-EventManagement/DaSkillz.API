using Domain.Entities;
using Domain.Enum.Price;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PriceRepository : RepositoryBase<Price>, IPriceRepository
    {
        private readonly ApplicationDbContext _context;

        public PriceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Price>> GetAllPrice(GetAllPriceOrderBy orderBy, bool isAsc)
        {
            var result = _context.Prices.AsQueryable();
            if (orderBy.Equals(GetAllPriceOrderBy.amount))
            {
                result = isAsc ? result.OrderBy(r => r.amount) : result.OrderByDescending(r => r.amount);
            }
            if (orderBy.Equals(GetAllPriceOrderBy.PriceType))
            {
                result = isAsc ? result.OrderBy(r => r.PriceType) : result.OrderByDescending(r => r.PriceType);
            }
            if (orderBy.Equals(GetAllPriceOrderBy.UpdateAt))
            {
                result = isAsc ? result.OrderBy(r => r.UpdatedAt) : result.OrderByDescending(r => r.UpdatedAt);
            }
            if (orderBy.Equals(GetAllPriceOrderBy.CreateAt))
            {
                result = isAsc ? result.OrderBy(r => r.CreatedAt) : result.OrderByDescending(r => r.CreatedAt);
            }
            return await result.ToListAsync();
        }

        public async Task<Price> GetPriceAdvertised()
        {
            var advertisementPrice = await _context.Prices
            .Where(p => p.PriceType == "advertisement" && p.status == "active")
            .FirstOrDefaultAsync();
            return advertisementPrice;
        }
    }
}
