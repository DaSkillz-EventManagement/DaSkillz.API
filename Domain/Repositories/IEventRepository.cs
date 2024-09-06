using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        // get Event
        //public Task<Event> getAllEventInfo(Guid eventId);
        public Task<PagedList<Event>> getEventByUserRole(EventRole eventRole, Guid userId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByTag(int tagId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByListTags(List<int> tagIds, int pageNo, int elementEachPage);
        
        Task<List<Event>> GetUserHostEvent(Guid userId);
    }
}
