using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IQuestionRepository: IRepository<Question>
{
    Task<List<Question>> GetQuestionsByQuizId(Guid quizId);
}
