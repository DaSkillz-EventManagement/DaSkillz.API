using Domain.Entities;
using Domain.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAdvertisementRepository : IRepository<Advertisement>
    {
        Task<List<Guid>> GetAllEventIdsAsync();
    }
}
