using Application.Helper;
using Domain.DTOs.Events;
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

        private async Task<IQueryable<Event>> GetUserRegisterdEventsQuery(Guid userId)
        {
            var participants = await _context.Participants
                .Where(p => p.UserId.Equals(userId))
                .Select(p => p.EventId)
                .ToListAsync();

            return _context.Events.Include(e => e.Participants)
                .Where(e => participants.Contains(e.Id))
                .AsNoTracking();
        }



        public Task<PagedList<Event>> GetFilteredEvent(EventFilterObjectDto filter, int pageNo, int elementEachPage)
        {
            throw new NotImplementedException();
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

        public Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObjectDto filter, Guid userId, int pageNo, int elementEachPage)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsOwner(Guid userId, Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id.Equals(eventId) && e.CreatedBy.Equals(userId));
        }
        public async Task<List<Event>> UserPastEvents(Guid userId)
        {

            var eventResponse = await _context.Events
                .Where(e => e.CreatedBy == userId && DateTimeHelper.ToDateTime(e.EndDate) < DateTime.Now)
                .ToListAsync();
            var events = await GetUserRegisterdEventsQuery(userId);
            var eventList = await events.Where(e => DateTimeHelper.ToDateTime(e.EndDate) < DateTime.Now)
                .OrderByDescending(e => e.EndDate).ToListAsync();
            return eventResponse.Concat(eventList).DistinctBy(e => e.Id).ToList();
        }

        public async Task<List<Event>> UserIncomingEvents(Guid userId)
        {
            var eventResponse = await _context.Events
                .Where(e => e.CreatedBy == userId && DateTimeHelper.ToDateTime(e.StartDate) >= DateTime.Now)
                .ToListAsync();
            var incomingEvents = await GetUserRegisterdEventsQuery(userId);
            var eventList = await incomingEvents.Where(e => DateTimeHelper.ToDateTime(e.StartDate) >= DateTime.Now)
                .OrderByDescending(e => e.StartDate).ToListAsync();
            return eventResponse.Concat(eventList).DistinctBy(e => e.Id).ToList();
        }

        public bool UpdateEventStatusToOnGoing(Guid eventId)
        {
            var ongoingEvent = _context.Events.Find(eventId);
            ongoingEvent.Status = EventStatus.OnGoing.ToString();
            _context.Update(ongoingEvent);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateEventStatusToOnGoing()
        {
            var ongoingEvents = _context.Events.Where(e => e.StartDate <= DateTimeHelper.ToJsDateType(DateTime.Now) &&
            e.Status!.Equals(EventStatus.NotYet.ToString())).ToList();
            ongoingEvents.ForEach(e => e.Status = EventStatus.OnGoing.ToString());
            _context.UpdateRange(ongoingEvents);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateEventStatusToEnded(Guid eventId)
        {
            var endedEvent = _context.Events.Find(eventId);
            endedEvent.Status = EventStatus.Ended.ToString();
            _context.Update(endedEvent);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateEventStatusToEnded()
        {
            var endedEvents = _context.Events.Where(e => e.EndDate <= DateTimeHelper.ToJsDateType(DateTime.Now) &&
            e.Status!.Equals(EventStatus.OnGoing.ToString())).ToList();
            endedEvents.ForEach(e => e.Status = EventStatus.Ended.ToString());
            _context.UpdateRange(endedEvents);
            return _context.SaveChanges() > 0;
        }
    }
}
