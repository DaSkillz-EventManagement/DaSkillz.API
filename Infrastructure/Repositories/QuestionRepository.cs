using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    private readonly ApplicationDbContext _context;
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> CountQuestion(Guid quizId)
    {
        return await _context.Questions.Where(q => q.QuizId == quizId).CountAsync();
    }

    public async Task<List<Question>> DeleteQuestions(List<Guid> questionIds)
    {
        List <Question> result = new List<Question> ();
        var questions = await _context.Questions
        .Include(q => q.Answers)
        .Where(q => questionIds.Contains(q.QuestionId))
        .ToListAsync();

        var userAnswers = await _context.UserAnswers
        .Where(ua => questionIds.Contains(ua.QuestionId))
        .ToListAsync();
        foreach (var question in questions)
        {
            result.Add(question);
            var relatedUserAnswers = userAnswers.Where(ua => ua.QuestionId == question.QuestionId).ToList();
            _context.UserAnswers.RemoveRange(relatedUserAnswers);
        }
        _context.Questions.RemoveRange(questions);
        await _context.SaveChangesAsync();

        return result;
        /*foreach (var id in questionIds) 
        {
            
            var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.QuestionId == id);
            if(question != null)
            {
                var userAnswer = await _context.UserAnswers.FirstOrDefaultAsync(q => q.QuestionId == id);
                if(userAnswer != null)
                {
                    _context.UserAnswers.Remove(userAnswer);
                }
                result.Add(question!);
                _context.Questions.Remove(question);
            }
        }
        await _context.SaveChangesAsync();
        return result;*/
    }

    public async Task<Question?> GetQuestionById(Guid questionId)
    {
        return await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.QuestionId == questionId);
    }

    public async Task<List<Question>> GetQuestionsByQuizId(Guid quizId)
    {
        var questions = await _context.Questions.Include(q => q.Answers).Where(q => q.QuizId == quizId).ToListAsync();
        return questions;
    }
}
