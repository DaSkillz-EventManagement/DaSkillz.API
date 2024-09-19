using Application.Helper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.User.Response;
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

        private IQueryable<Event> ApplyFilter(IQueryable<Event> eventList, EventFilterObjectDto filter)
        {
            if (filter.TagId != null)
            {
                var tags = _context.Tags.Where(t => filter.TagId.Contains(t.TagId)).ToList();
                eventList = eventList.Where(e => e.Tags.Any(t => tags.Contains(t)));
            }
            if (!string.IsNullOrWhiteSpace(filter.EventName))

            {
                eventList = from e in eventList
                            where e.EventName!.Contains(filter.EventName)
                            select e;
            }
            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                eventList = from e in eventList
                            where (e.Location!.Contains(filter.Location)) || (e.LocationAddress!.Contains(filter.Location))
                            select e;
            }
            if (!string.IsNullOrWhiteSpace(filter.LocationId))
            {
                eventList = from e in eventList
                            where (e.LocationId!.Equals(filter.LocationId))
                            select e;
            }
            if (filter.StartDateFrom != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.StartDateFrom);
                // get DateTime from DateTimeOffset
                var startDateFrom = DateTimeHelper.GetTimeAsLong(dateTimeOffset);
                eventList = from e in eventList
                            where e.StartDate >= startDateFrom
                            select e;
            }
            if (filter.StartDateTo != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.StartDateTo);
                // get DateTime from DateTimeOffset
                var startDateTo = DateTimeHelper.GetTimeAsLong(dateTimeOffset);
                eventList = from e in eventList
                            where e.StartDate <= startDateTo
                            select e;
            }
            if (filter.EndDateFrom != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.EndDateFrom);
                // get DateTime from DateTimeOffset
                var endDateFrom = DateTimeHelper.GetTimeAsLong(dateTimeOffset);
                eventList = from e in eventList
                            where e.EndDate >= endDateFrom
                            select e;

            }
            if ((filter.EndDateTo != null))
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.EndDateTo);
                // get DateTime from DateTimeOffset
                var endDateTo = DateTimeHelper.GetTimeAsLong(dateTimeOffset);
                eventList = from e in eventList
                            where e.EndDate <= endDateTo
                            select e;
            }
            {

            }
            if (filter.Status != null)
            {
                eventList = eventList.Where(e => filter.Status.Contains(e.Status!));
            }
            if (filter.TicketFrom != null)
            {
                eventList = from e in eventList
                            where e.Fare >= filter.TicketFrom
                            select e;
            }
            if (filter.TicketTo != null)
            {
                eventList = from e in eventList
                            where e.Fare <= filter.TicketTo
                            select e;
            }
            if (filter.Approval != null)
            {
                eventList = from e in eventList
                            where e.Approval == filter.Approval
                            select e;
            }

            //eventList = filter.IsAscending ? eventList.OrderBy(s => s.EndDate) : eventList.OrderByDescending(s => s.StartDate);               
            return eventList;
        }

        private List<Event> FilterEventsByDistance(List<Event> events, string userCoord)
        {
            List<Event> result = new List<Event>();

            if (!string.IsNullOrWhiteSpace(userCoord))
            {
                string[] userCoordParts = userCoord.Split(',');
                double userLat = double.Parse(userCoordParts[0].Trim());
                double userLong = double.Parse(userCoordParts[1].TrimStart());

                foreach (var item in events)
                {
                    string[] eventCoordParts = item.LocationCoord!.Split(',');
                    double eventLat = double.Parse(eventCoordParts[0].Trim());
                    double eventLong = double.Parse(eventCoordParts[1].TrimStart());
                    double distance = CalculateDistance(userLat, userLong, eventLat, eventLong);

                    if (distance <= 5000)
                    {
                        Console.WriteLine($"event {item.EventName} distance: " + distance);
                        result.Add(item);
                    }
                }
            }
            else
            {
                result = events.ToList();
            }

            return result;
        }

        double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371.0; // Earth's radius in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;
            return distance * 1000;//convert from kilometer to meter
        }
        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public async Task<PagedList<Event>> GetFilteredEvent(EventFilterObjectDto filter, int pageNo, int elementEachPage)
        {
            var totalEle = await _context.Events.CountAsync();
            var eventList = _context.Events.Include(e => e.Tags).AsQueryable();
            eventList = ApplyFilter(eventList, filter).PaginateAndSort(pageNo, elementEachPage, filter.SortBy ?? "StartDate", filter.IsAscending);
            //eventList = ApplyFilter(eventList, filter).Skip(skipElements).Take(elementEachPage);
            List<Event> temp = await eventList.ToListAsync();
            List<Event> result = new List<Event>();
            if (!string.IsNullOrWhiteSpace(filter.UserCoord))
            {
                result = FilterEventsByDistance(temp, filter.UserCoord);
            }
            else
            {
                result = temp;
            }
            return new PagedList<Event>(result, totalEle, pageNo, elementEachPage);
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
        /*public async Task<bool> IsOwner(Guid userId, Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id.Equals(eventId) && e.CreatedBy.Equals(userId));
        }*/
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

        public async Task<bool> IsOwner(Guid userId, Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id.Equals(eventId) && e.CreatedBy.Equals(userId));
        }

        public async Task<bool> ChangeEventStatus(Guid eventId, EventStatus status)
        {
            var deleteEvent = await _context.Events.FindAsync(eventId);
            deleteEvent!.Status = status.ToString();
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<EventCreatorLeaderBoardDto>> GetTop10CreatorsByEventCount()
        {
            List<EventCreatorLeaderBoardDto> userInfos = new List<EventCreatorLeaderBoardDto>();
            var result = _context.Events
                .AsEnumerable()
                .GroupBy(e => e.CreatedBy!)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => _context.Users.Find(g.Key)!.UserId.ToString(), g => g.Count());
            foreach (var item in result)
            {
                var temp = _context.Users.Find(Guid.Parse(item.Key!));
                var userInfo = new EventCreatorLeaderBoardDto();
                userInfo.totalevent = item.Value;
                userInfo.FullName = temp.FullName;
                userInfo.Avatar = temp.Avatar;
                userInfo.userId = temp.UserId;
                userInfos.Add(userInfo);
            }
            return userInfos;
        }



        public async Task<List<EventLocationLeaderBoardDto>> GetTop10LocationByEventCount()
        {
            List<EventLocationLeaderBoardDto> result = new List<EventLocationLeaderBoardDto>();
            var temp = _context.Events
                .AsEnumerable().Where(e => e.Status!.Equals(EventStatus.NotYet.ToString()))
                .GroupBy(e => e.Location!)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => g.Key, g => g.Count());
            foreach (var item in temp)
            {
                EventLocationLeaderBoardDto locationInfo = new EventLocationLeaderBoardDto();
                var eventTemp = _context.Events.FirstOrDefault(e => e.Location.Equals(item.Key));
                locationInfo.totalevent = item.Value;
                if (eventTemp!.Location!.Equals("Google meet", StringComparison.OrdinalIgnoreCase))
                {
                    locationInfo.Location = "Online";
                }
                else
                {
                    locationInfo.Location = eventTemp!.Location;
                }
                locationInfo.LocationId = eventTemp.LocationId;
                locationInfo.LocationUrl = eventTemp.LocationUrl;
                locationInfo.LocationCoord = eventTemp.LocationCoord;
                locationInfo.LocationAddress = eventTemp.LocationAddress;
                /*foreach(string item2 in vietnamAdministrativeDivisions)
                {
                    if (eventTemp!.LocationAddress.Contains(item2))
                    {
                        locationInfo.city = item2;
                    }
                }*/
                result.Add(locationInfo);
            }
            return result!;
        }

        public Task<List<Event>> GetEventsByIdsAsync(List<Guid> eventIds)
        {
            throw new NotImplementedException();
        }

        public async Task<CreatedByUserDto> GetHostInfo(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            CreatedByUserDto response = new CreatedByUserDto();
            response.avatar = user!.Avatar;
            response.Id = user.UserId;
            response.Name = user.FullName;
            return response;
        }
    }
}
