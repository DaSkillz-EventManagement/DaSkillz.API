using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IUserAnswerRepository : IRepository<UserAnswer>
{
    Task<bool> IsAttempted(Guid quizId, Guid userId);
    Task<int> GetAttemptNo(Guid quizId, Guid userId);
    Task<List<User>> GetListUsersAttemptedQuiz(Guid quizId);
    Task<List<UserAnswer>> GetUserAnswer(Guid userId, Guid quizId);
}
