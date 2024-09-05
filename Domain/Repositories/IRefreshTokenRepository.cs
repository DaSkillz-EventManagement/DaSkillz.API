using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetTokenAsync(string refreshToken);
        Task<RefreshToken?> GetUserByIdAsync(Guid id);
        Task<bool> AddRefreshToken(RefreshToken newRefreshToken);
        Task<bool> RemoveRefreshTokenAsync(string token);
    }
}
