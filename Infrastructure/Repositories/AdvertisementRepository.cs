using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdvertisementRepository : RepositoryBase<Advertisement>, IAdvertisementRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvertisementRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Guid>> GetAllEventIdsAsync()
        {
            return await _context.Set<Advertisement>()
                             .Select(ad => ad.EventId)
                             .ToListAsync();
        }
    }
}
