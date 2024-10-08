using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Participant;
using Domain.Models.Pagination;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch.Security;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<DailyParticipation>> GetParticipationByDayAsync(Guid? userId, Guid? eventId, DateTime startDate, DateTime endDate)
    {
        DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);

        var query = _context.Participants
            .Include(a => a.Event)
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDateAdjusted);

        var isRole = await _context.Users.AnyAsync(x => x.RoleId == 2 && x.UserId == userId);
        if (isRole)
        {
            if (eventId != null)
            {
                query = query.Where(p => p.EventId == eventId.Value && p.Event.CreatedBy == userId);
            }
            else
            {
                query = query.Where(p => p.Event.CreatedBy == userId);
            }
        }

        else 
        {
            query = query.Where(p => p.EventId == eventId.Value);
        }

        var participationByDay = await query
            .GroupBy(p => p.CreatedAt!.Value.Date)
            .Select(g => new DailyParticipation
            {
                Date = g.Key.ToString("dd/MM/yyyy"),
                Count = g.Count()
            })
            .ToListAsync();

        return participationByDay;
    }

    public async Task<List<HourlyPartitipant>> GetParticipationByHourAsync(Guid? userId, Guid? eventId, DateTime startDate, DateTime endDate)
    {
        DateTime endDateAdjusted = endDate.Date.AddDays(1).AddTicks(-1);

        // Fetch the participants first
        var participants = await _context.Participants
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDateAdjusted)
            .ToListAsync(); // Get all relevant records

        var isRole = await _context.Users.AnyAsync(x => x.RoleId == 2 && x.UserId == userId);
        if (isRole)
        {
            if (eventId != null)
            {
                participants = participants.Where(p => p.EventId == eventId.Value && p.Event.CreatedBy == userId).ToList();
            }
            else
            {
                participants = participants.Where(p => p.Event.CreatedBy == userId).ToList();
            }
        }

        else if (eventId != null)
        {
            participants = participants.Where(p => p.EventId == eventId.Value).ToList();
        }

        var hourlyParticipants = participants
            .GroupBy(p => new
            {
                Day = p.CreatedAt!.Value.Date,
                Hour = p.CreatedAt.Value.Hour
            })
            .Select(g => new HourlyPartitipant
            {
                Date = g.Key.Day.ToString("dd/MM/yyyy"), // Perform formatting in memory
                Hour = string.Format("{0:D2}:00", g.Key.Hour),
                Count = g.Count() // Count participants
            })
            .OrderBy(e => e.Date)
            .ToList();

        return hourlyParticipants;
    }


    public async Task<bool> IsExistedOnEvent(Guid userId, Guid eventId)
    {
        return await _context.Participants.AnyAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
    }

    public async Task<Participant?> GetParticipant(Guid userId, Guid eventId)
    {
        return await _context.Participants.FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
    }

    public async Task<IEnumerable<Participant?>> GetParticipantsByEventId(Guid eventId)
    {
        return await _context.Participants.Where(p => p.EventId.Equals(eventId)).ToListAsync();
    }

    public async Task<Participant?> GetDetailParticipant(Guid userId, Guid eventId)
    {
        return await _context.Participants
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
    }

    public async Task<PagedList<Participant>> FilterDataParticipant(FilterParticipantDto filter)
    {
        var query = _context.Participants.Where(p => p.EventId.Equals(filter.EventId) && p.Status!.Equals(filter.Status.ToString())).AsNoTracking().AsQueryable();

        if (filter.RoleEventId.HasValue)
        {
            query = query.Where(p => p.RoleEventId == filter.RoleEventId);
        }

        if (filter.CheckedIn.HasValue)
        {
            query = query.Where(p => p.CheckedIn <= filter.CheckedIn);
        }

        if (filter.IsCheckedMail.HasValue)
        {
            query = query.Where(p => p.IsCheckedMail == filter.IsCheckedMail);
        }

        if (filter.CreatedAt.HasValue)
        {
            query = query.Where(p => p.CreatedAt <= filter.CreatedAt);
        }

        query = query.Include(p => p.User);
        query = SortParticipants(query, filter.SortBy);

        return await query.ToPagedListAsync(filter.Page, filter.EachPage);
    }



    public new async Task<PagedList<Participant>> GetAll(Expression<Func<Participant, bool>> predicate, int page, int eachPage, string sortBy, bool isAscending = true)
    {
        var entities = await _context.Participants
            .Include(p => p.User)
            .Where(predicate).PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

        return new PagedList<Participant>(entities, entities.Count, page, eachPage);

    }

    public new async Task<IEnumerable<Participant>> GetAll(Expression<Func<Participant, bool>> predicate)
    {
        var entities = await _context.Participants
            .Include(p => p.Event)
            .Include(p => p.User)
            .Where(predicate)
            .ToListAsync();

        return entities;
    }

    private static IQueryable<Participant> SortParticipants(IQueryable<Participant> participants, ParticipantSortBy sortBy)
    {
        switch (sortBy)
        {
            case ParticipantSortBy.CheckedIn:
                participants = participants.OrderByDescending(p => p.CheckedIn);
                break;
            case ParticipantSortBy.CreatedAt:
                participants = participants.OrderBy(p => p.CreatedAt);
                break;
            default:
                break;
        }

        return participants;
    }

    public async Task UpSert(Participant participant)
    {
        var participantExist = _context.Participants.FirstOrDefault(p => p.UserId.Equals(participant.UserId) && p.EventId.Equals(participant.EventId));

        if (participantExist == null)
        {
            await _context.Participants.AddAsync(participant);
        }
        else
        {
            participantExist.RoleEventId = participant.RoleEventId;
            participantExist.Status = participant.Status;
            _context.Participants.Update(participantExist);
        }

    }

    public async Task<bool> IsRole(Guid userId, Guid eventId, Domain.Enum.Events.EventRole role)
    {
        return await _context.Participants.AnyAsync(p => p.EventId.Equals(eventId) && p.UserId.Equals(userId) && p.RoleEventId.Equals((int)role));
    }

    public async Task<bool> IsReachedCapacity(Guid eventId)
    {
        int epCount = await _context.Participants.Where(p => p.EventId == eventId).CountAsync();
        Event entity = await _context.Events.FindAsync(eventId);
        int eventCapacity = entity!.Capacity!.Value;
        if (eventCapacity < 0)
        {
            return false;
        }
        return epCount > eventCapacity;
    }
}
