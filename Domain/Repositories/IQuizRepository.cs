using Domain.Entities;
using Domain.Enum.Quiz;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<List<Quiz>> GetAllQuizsByEventId(Guid eventId, QuizEnum? status);
    Task<int> GetQuizAttemptAllow(Guid quizId);
}
