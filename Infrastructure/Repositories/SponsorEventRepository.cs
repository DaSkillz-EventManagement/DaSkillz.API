using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Enum.Sponsor;
using Domain.Models.Pagination;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class SponsorEventRepository : RepositoryBase<SponsorEvent>, ISponsorEventRepository
    {
        private readonly ApplicationDbContext _context;

        public SponsorEventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsSponsored(Guid eventId)
        {
            var sponsorList = await _context.SponsorEvents.Where(s => s.EventId == eventId
            && s.Status!.Equals(SponsorRequest.Confirmed.ToString())).ToListAsync();
            return sponsorList.Any();
        }
        public async Task<SponsorEvent?> CheckSponsoredEvent(Guid eventId, Guid userId)
        {
            return await _context.SponsorEvents
                         .FirstOrDefaultAsync(s => s.EventId == eventId
                                                && s.UserId == userId
                                                && s.Status!.Equals(SponsorRequest.Confirmed.ToString()));
        }

        public async Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId)
        {
            return await _context.SponsorEvents
                         .FirstOrDefaultAsync(s => s.EventId == eventId
                                                && s.UserId == userId
                                                );

        }

        public async Task<SponsorEvent?> DeleteSponsorRequest(Guid eventId, Guid userId)
        {
            var sponsorRequest = await CheckSponsorEvent(eventId, userId);
            _context.SponsorEvents.Remove(sponsorRequest!);
            await _context.SaveChangesAsync();
            return sponsorRequest;
        }

        public async Task<List<SponsorEvent>> GetRequestSponsor(Guid userId, string? status, int page, int eachPage)
        {
            var list = _context.SponsorEvents.Include(se => se.User).Where(s => s.UserId.Equals(userId));
            if (status != null)
            {
                list = list.Where(p => p.Status!.Equals(status));
            }
            //list = list.Include(p => p.Event);
            list = list.Skip(page -1).Take(eachPage).OrderByDescending(p => p.UpdatedAt);
            return await list.ToListAsync();
        }
        public async Task<int> GetRequestSponsorCount(Guid userId, string? status)
        {
            var list = _context.SponsorEvents.Include(se => se.User).Where(s => s.UserId.Equals(userId));
            if (status != null)
            {
                list = list.Where(p => p.Status!.Equals(status));
            }
            //list = list.Include(p => p.Event);
            return await list.CountAsync();
        }

        public async Task<List<SponsorEvent>> GetSponsorEvents(SponsorEventFilterDto sponsorFilter)
        {
            var list = _context.SponsorEvents.Include(sp => sp.User).Where(s => s.EventId.Equals(sponsorFilter.EventId)).OrderByDescending(p => p.CreatedAt).AsNoTracking().AsQueryable();

            if (sponsorFilter.Status != null)
            {
                list = list.Where(s => s.Status!.Equals(sponsorFilter.Status));
            }

            if (sponsorFilter.IsSponsored.HasValue)
            {
                list = list.Where(s => s.IsSponsored == sponsorFilter.IsSponsored);
            }
            if (sponsorFilter.SponsorType != null)
            {
                list = list.Where(s => s.SponsorType!.Equals(sponsorFilter.SponsorType));
            }
            //list = list.Include(p => p.User);
            return await list.Skip(sponsorFilter.Page -1).Take(sponsorFilter.EachPage).ToListAsync();
        }
        public async Task<int> GetSponsorEventsCount(SponsorEventFilterDto sponsorFilter)
        {
            var list = _context.SponsorEvents.Include(sp => sp.User).Where(s => s.EventId.Equals(sponsorFilter.EventId)).OrderByDescending(p => p.CreatedAt).AsNoTracking().AsQueryable();

            if (sponsorFilter.Status != null)
            {
                list = list.Where(s => s.Status!.Equals(sponsorFilter.Status));
            }

            if (sponsorFilter.IsSponsored.HasValue)
            {
                list = list.Where(s => s.IsSponsored == sponsorFilter.IsSponsored);
            }
            if (sponsorFilter.SponsorType != null)
            {
                list = list.Where(s => s.SponsorType!.Equals(sponsorFilter.SponsorType));
            }
            //list = list.Include(p => p.User);
            return await list.CountAsync();
        }
    }
}
