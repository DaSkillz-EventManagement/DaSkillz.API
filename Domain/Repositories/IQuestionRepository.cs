using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IQuestionRepository : IRepository<Question>
{
    Task<List<Question>> GetQuestionsByQuizId(Guid quizId);
    Task<List<Question>> DeleteQuestions(List<Guid> quetionIds);

    Task<Question?> GetQuestionById(Guid questionId);
}
