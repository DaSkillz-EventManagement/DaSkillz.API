using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> CheckUserIsPremium(Guid? userId);
        Task<IEnumerable<User>> GetUsersByKeywordAsync(string keyword);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> AddUser(User newUser);
        User? GetByEmail(string email);
        User? GetUserById(Guid userId);
        Task<IEnumerable<User>> GetAllUser(int page, int eachPage, string sortBy, bool isAscending = false);
        Task<IEnumerable<IGrouping<int, User>>> GetUsersCreatedInMonthAsync(int year);
        Task<int> GetTotalUsersAsync();
        Task<IEnumerable<User>> UpdateIsPremiumUser();
        Task<IEnumerable<User>> FilterUsersAsync(Guid? userId = null, string? fullName = null, string? email = null, string? phone = null, string? status = null);
    }
}
