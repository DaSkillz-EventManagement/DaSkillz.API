using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories;

public class UserAnswerRepository: RepositoryBase<UserAnswer>, IUserAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public UserAnswerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
