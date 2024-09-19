using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User? GetUserById(Guid userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId.Equals(userId));
        }

        public async Task<IEnumerable<User>> GetUsersByKeywordAsync(string keyword)
        {
            return await _context.Users.Where(a => a.Email!.StartsWith(keyword) || a.Email.Contains(keyword)).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId!.Equals(userId));
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(a => a.Role).FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email.ToLower()));
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email!.ToLower().Equals(email.ToLower()));
        }

        public async Task<bool> AddUser(User newUser)
        {
            if (_context.Users.Any(x => x.Email!.Equals(newUser.Email)))
            {
                return false;
            }
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUser(int page, int pagesize, string sortBy, bool isAscending = false)
        {
            //var cacheKey = $"GetAllUser_{page}_{pagesize}_{sortBy}_{isAscending}";
            var entities = await _context.Users.Include(a => a.Role).PaginateAndSort(page, pagesize, sortBy, isAscending).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<IGrouping<int, User>>> GetUsersCreatedInMonthAsync(int year)
        {
            var users = await _context.Users
                .Where(u => u.CreatedAt.HasValue && u.CreatedAt.Value.Year == year)
            .ToListAsync();

            var result = users
                .GroupBy(u => u.CreatedAt!.Value.Month)
                .OrderBy(g => g.Key);

            return result;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            var totalUsers = await _context.Users.CountAsync();

            return totalUsers;
        }

    }
}
