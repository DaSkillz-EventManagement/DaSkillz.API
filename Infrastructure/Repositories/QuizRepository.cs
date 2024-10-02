using Domain.Entities;
using Domain.Enum.Quiz;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
{
    private readonly ApplicationDbContext _context;

    public QuizRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Quiz>> GetAllQuizsByEventId(Guid eventId, QuizEnum? status)
    {
        var quizs = _context.Quizs.Where(q => q.eventId == eventId).AsQueryable();

        if(status != null)
        {
            quizs = quizs.Where(q => q.QuizStatus == (int)status!);
        }

        return await quizs.ToListAsync();
    }

    public async Task<int> GetQuizAttemptAllow(Guid quizId)
    {
        Quiz quiz = await _context.Quizs.FindAsync(quizId);
        return quiz == null ? -1 : quiz.AttemptAllow;
    }
}
