using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Participant;
using Domain.Models.Pagination;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
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
        return await _context.Participants.Where(p => p.EventId.Equals(eventId) ).ToListAsync();
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
        if(eventCapacity < 0)
        {
            return false;
        }
        return epCount > eventCapacity;
    }
}
