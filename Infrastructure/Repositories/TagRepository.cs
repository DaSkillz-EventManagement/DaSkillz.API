using Domain.Entities;
using Domain.Enum.Events;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Tag> GetTagByName(string name)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName.ToUpper().Trim().Equals(name.ToUpper().Trim()));
            if (tag != null)
            {
                return tag;
            }
            return null;
        }

        public async Task<List<Tag>> SearchTag(string searchTerm)
        {
            return await _context.Tags.Where(p => p.TagName.Contains(searchTerm)).ToListAsync();
        }
        public async Task<List<Tag>> TrendingTag()
        {
            var tags = await _context.Tags.Include(t => t.Events)
            .Where(t => t.Events.Any(e => e.Status == EventStatus.NotYet.ToString()))
            .GroupBy(t => new { t.TagId, t.TagName })
                .OrderByDescending(g => g.Count())
                .Select(g => new Tag
                {
                    TagId = g.Key.TagId,
                    TagName = g.Key.TagName!
                })
            .ToListAsync();

            return tags;
        }
    }
}
