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
            return _context.Users.Find(userId);
        }

        public async Task<bool> CheckUserIsPremium(Guid? userId)
        {
            return await _context.Users.AnyAsync(a => a.UserId == userId && a.IsPremiumUser);
        }

        public async Task<IEnumerable<User>> GetUsersByKeywordAsync(string keyword)
        {
            return await _context.Users.Include(a => a.Subscription).Where(a => a.Email!.StartsWith(keyword) || a.Email.Contains(keyword)).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.Include(a => a.Subscription).FirstOrDefaultAsync(x => x.UserId!.Equals(userId));
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
            var entities = await _context.Users.
                Include(a => a.Role).
                Include(a => a.Subscription).
                AsSplitQuery().
                PaginateAndSort(page, pagesize, sortBy, isAscending).ToListAsync();
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


        public async Task<IEnumerable<User>> UpdateIsPremiumUser()
        {
            return await _context.Users
                    .Where(u => u.Subscription != null)
                     .ToListAsync();
        }

        public async Task<bool> IsPremiumAccount(Guid userId)
        {
            return await _context.Users.AnyAsync(e => e.UserId.Equals(userId) && e.IsPremiumUser);
        }

        public async Task<IEnumerable<User>> FilterUsersAsync(Guid? userId = null, string? fullName = null, string? email = null, string? phone = null, string? status = null)
        {
            // Bắt đầu truy vấn từ bảng Users
            var query = _context.Users.AsQueryable();

            // Kiểm tra và áp dụng các điều kiện lọc
            if (userId.HasValue)
            {
                query = query.Where(u => u.UserId == userId.Value);
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.Where(u => u.FullName != null && u.FullName.Contains(fullName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email != null && u.Email.ToLower().Contains(email.ToLower()));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(u => u.Phone != null && u.Phone.Contains(phone));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(u => u.Status.Equals(status));
            }

            // Thực hiện truy vấn và trả về danh sách người dùng đã lọc
            return await query.Include(a => a.Role)
                               .Include(a => a.Subscription)
                               .ToListAsync();
        }

    }
}
