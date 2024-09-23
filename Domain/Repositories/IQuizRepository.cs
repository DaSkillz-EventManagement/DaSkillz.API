using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IQuizRepository: IRepository<Quiz>
{
    Task<List<Quiz>> GetAllQuizsByEventId(Guid eventId);
}
