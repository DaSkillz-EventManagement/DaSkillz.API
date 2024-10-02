using Domain.DTOs.User.Response;
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
        .FirstOrDefaultAsync();

        return userAttempt?.AttemptNo ?? 0;    
    }
    private async Task<List<Guid>> GetUsersAttemptedQuiz(Guid quizId)
    {
        return await _context.UserAnswers
            .Where(ua => ua.QuizId == quizId)
            .Select(ua => ua.UserId)
            .Distinct()
            .ToListAsync();
    }
    public async Task<List<User>> GetListUsersAttemptedQuiz(Guid quizId)
    {
        List<Guid> userIds = await GetUsersAttemptedQuiz (quizId);
        var users = await _context.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
        return users;
    }

    public async Task<List<UserAnswer>> GetUserAnswer(Guid userId, Guid quizId)
    {
        var result = await _context.UserAnswers.Where(u => u.QuizId == quizId && u.UserId == userId)
            .ToListAsync();
        return result;
    }
}
