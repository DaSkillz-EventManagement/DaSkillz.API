using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class AdvertisedEventRepository : RepositoryBase<AdvertisedEvent>, IAdvertisedEventRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvertisedEventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
