using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbackRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Feedback>?> GetFeedbackByEventIdAndStar(Guid eventId, int? numOfStar, int page, int eachPage)
    {
        var list = _context.Feedbacks.Include(f => f.User).Where(f => f.EventId.Equals(eventId));
        if (numOfStar.HasValue)
        {
            list = list.Where(f => f.Rating == numOfStar.Value);
        }
        list.OrderByDescending(f => f.CreatedAt);

        return await list.ToPagedListAsync(page, eachPage);
    }
    public async Task<Feedback> GetUserEventFeedback(Guid eventId, Guid userId)
    {
        var response = await _context.Feedbacks.FirstOrDefaultAsync(f => f.UserId == userId && f.EventId == eventId);
        return response!;
    }
    public async Task<PagedList<Feedback>?> GetAllUserFeebacks(Guid userId, int page, int eachPage)
    {
        var result = _context.Feedbacks.Where(f => f.UserId == userId);
        return await result.ToPagedListAsync(page, eachPage);
    }
}
