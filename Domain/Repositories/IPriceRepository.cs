using Domain.Entities;
using Domain.Enum.Price;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IPriceRepository : IRepository<Price>
{
    Task<List<Price>> GetAllPrice(GetAllPriceOrderBy orderBy, bool isAsc);
    Task<Price> GetAllPriceAdvertised();
}
