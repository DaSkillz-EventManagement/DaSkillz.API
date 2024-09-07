using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ILogoRepository : IRepository<Logo>
    {
        Task<Logo> GetByName(string name);
    }
}
