using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IAdvertisedEventRepository : IRepository<AdvertisedEvent>
    {
        Task<List<Guid>> GetListAdvertisedEventId();
        Task<List<AdvertisedEvent?>> GetAdvertisedByEventId(Guid eventId);
        Task<List<AdvertisedEvent>> GetFilteredAdvertisedByHost(Guid userId, string status);
        Task<AdvertisedEvent?> GetLastestAdvertisedEvent(Guid eventId);
    }
}
