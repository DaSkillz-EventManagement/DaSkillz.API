using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LogoRepository : RepositoryBase<Logo>, ILogoRepository
    {
        private readonly ApplicationDbContext _context;

        public LogoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Logo> GetByName(string name)
        {
            Logo? logo = await _context.Logos.FirstOrDefaultAsync(lo => lo.SponsorBrand == name);
            return logo != null ? logo : null;
        }
    }
}
