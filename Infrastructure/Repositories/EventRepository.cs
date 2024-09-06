using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //public Task<Event> getAllEventInfo(Guid eventId)
        //{
        //    throw new NotImplementedException();
        //}

        
        public Task<PagedList<Event>> getEventByUserRole(EventRole eventRole, Guid userId, int pageNo, int elementEachPage)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Event>> GetEventsByListTags(List<int> tagIds, int pageNo, int elementEachPage)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Event>> GetEventsByTag(int tagId, int pageNo, int elementEachPage)
        {
            throw new NotImplementedException();
        }

        public Task<List<Event>> GetUserHostEvent(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
