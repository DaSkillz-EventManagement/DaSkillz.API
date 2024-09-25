using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IAdvertisedEventRepository : IRepository<AdvertisedEvent>
    {
        Task<List<Guid>> GetListAdvertisedEventId();
    }
}
