using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class EventRepo : RepositoryBase<Event>, IEventRepo
    {
        private readonly ApplicationDbContext _context;

        public EventRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
