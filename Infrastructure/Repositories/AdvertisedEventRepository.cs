using Application.Helper;
using Domain.Entities;
using Domain.Enum.AdvertisedEvents;
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
            return await _context.AdvertisedEvents.Where(ad => ad.EventId.Equals(eventId)).AsNoTracking()
                   .FirstOrDefaultAsync();
        }

        public async Task<List<AdvertisedEvent>> GetFilteredAdvertisedByHost(Guid userId, string status)
        {
            var result = await _context.AdvertisedEvents.Where(ad => ad.UserId.Equals(userId)).ToListAsync();
            if (status.Equals(AdvertisedStatus.Expired.ToString()))
            {
                result = result.Where(ad => ad.EndDate < DateTimeHelper.GetCurrentTimeAsLong()).ToList();
            } else if (status.Equals(AdvertisedStatus.Remain.ToString()))
            {
                result = result.Where(ad => ad.EndDate > DateTimeHelper.GetCurrentTimeAsLong()).ToList();
            }
            return result;
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
