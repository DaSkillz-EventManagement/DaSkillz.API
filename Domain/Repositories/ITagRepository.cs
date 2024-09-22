using Domain.Entities;
using Domain.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag> GetTagByName(string name);
        Task<List<Tag>> SearchTag(string searchTerm);
        Task<List<Tag>> TrendingTag();
    }
}
