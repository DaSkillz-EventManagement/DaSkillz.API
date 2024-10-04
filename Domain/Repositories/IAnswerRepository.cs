using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IAnswerRepository : IRepository<Answer>
{
    Task<Answer?> GetByIdAsNoTracking(Guid id);
}
