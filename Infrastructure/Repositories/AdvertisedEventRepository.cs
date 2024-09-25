using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertisedEventRepository : RepositoryBase<AdvertisedEvent>, IAdvertisedEventRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvertisedEventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AdvertisedEvent?> GetAdvertisedByEventId(Guid eventId)
        {
            return await _context.AdvertisedEvents.Where(ad => ad.EventId.Equals(eventId)).FirstOrDefaultAsync();
        }

        public async Task<List<Guid>> GetListAdvertisedEventId()
        {
            return await _context.AdvertisedEvents
                         .OrderBy(ae => ae.CreatedDate)
                         .Select(ae => ae.EventId)  // Select the EventId column
                         .ToListAsync();
        }
    }
}
