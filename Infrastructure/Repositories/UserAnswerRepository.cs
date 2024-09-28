using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public UserAnswerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsAttempted(Guid quizId, Guid userId)
    {
        return await _context.UserAnswers.AnyAsync(u => u.QuizId == quizId && u.UserId == userId);  
    }
    public async Task<int> GetAttemptNo(Guid quizId, Guid userId)
    {
        var userAttempt = await _context.UserAnswers
        .Where(u => u.QuizId == quizId && u.UserId == userId)
        .OrderByDescending(u => u.AttemptNo)
        .FirstAsync();

        return userAttempt?.AttemptNo ?? 0;    
    }
}
