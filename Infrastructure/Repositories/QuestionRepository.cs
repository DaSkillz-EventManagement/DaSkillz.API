using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    private readonly ApplicationDbContext _context;
    public QuestionRepository(ApplicationDbContext context): base(context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetQuestionsByQuizId(Guid quizId)
    {
        var questions = await _context.Questions.Include(q => q.Answers).Where(q => q.QuizId == quizId).ToListAsync();
        return questions;
    }
}
