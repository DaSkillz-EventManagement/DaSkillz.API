using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IUserAnswerRepository : IRepository<UserAnswer>
{
    Task<bool> IsAttempted(Guid quizId);
}
