using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //public Task<Event> getAllEventInfo(Guid eventId)
        //{
        //    throw new NotImplementedException();
        //}

        
        public async Task<PagedList<Event>> getEventByUserRole(EventRole eventRole, Guid userId, int pageNo, int elementEachPage)
        {
            var userInfo = await _context.Participants.Where(p => p.UserId == userId && p.RoleEvent!.RoleEventId == ((int)eventRole))
               .ToListAsync();
            var eventIds = userInfo.Select(p => p.EventId).ToList();
            var events = await _context.Events
                .Where(e => eventIds.Contains(e.Id))
                .Where(e => e.Status!.Equals(EventStatus.OnGoing.ToString()) || e.Status!.Equals(EventStatus.NotYet.ToString()))
                .AsSplitQuery().AsNoTracking()
                .PaginateAndSort(pageNo, elementEachPage, "CreatedAt", false)
                    .ToListAsync();
            var totalEle = events.Count;
            return new PagedList<Event>(events, totalEle, pageNo, elementEachPage);
        }

        public async Task<PagedList<Event>> GetEventsByListTags(List<int> tagIds, int pageNo, int elementEachPage)
        {
            var tags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();

            if (tags.Count > 0)
            {
                var events = await _context.Events.Include(e => e.Tags)
                    .Where(e => e.Tags.Any(t => tags.Contains(t)))
                    .PaginateAndSort(pageNo, elementEachPage, "CreatedAt", false)
                    .ToListAsync();

                var totalEle = events.Count;
                return new PagedList<Event>(events, totalEle, pageNo, elementEachPage);
            }

            return new PagedList<Event>(new List<Event>(), 0, 0, 0);
        }

        public async Task<PagedList<Event>> GetEventsByTag(int tagId, int pageNo, int elementEachPage)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagId == tagId);

            if (tag != null)
            {
                var events = await _context.Events.Include(e => e.Tags).Where(e => e.Tags.Contains(tag))
                    .PaginateAndSort(pageNo, elementEachPage, "CreatedAt", false).ToListAsync();
                var totalEle = events.Count;
                return new PagedList<Event>(events, totalEle, pageNo, elementEachPage);
            }
            return new PagedList<Event>(new List<Event>(), 0, 0, 0);
        }

        public async Task<List<Event>> GetUserHostEvent(Guid userId)
        {
            var eventList = await _context.Events.Where(e => e.CreatedBy == userId)
                .OrderByDescending(e => e.StartDate).AsNoTracking().ToListAsync();
            return eventList;
        }
    }
}
