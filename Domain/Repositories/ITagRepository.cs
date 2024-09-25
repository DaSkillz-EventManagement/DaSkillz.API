using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag> GetTagByName(string name);
        Task<List<Tag>> SearchTag(string searchTerm);
        Task<List<Tag>> TrendingTag();
    }
}
