using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public AnswerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Answer?> GetByIdAsNoTracking(Guid id)
    {
        return await _context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.AnswerId == id);
    }
}
