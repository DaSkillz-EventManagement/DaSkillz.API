using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetUserByIdAsync(Guid id)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<RefreshToken?> GetTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<bool> AddRefreshToken(RefreshToken newRefreshToken)
        {
            if (_context.RefreshTokens.Any(x => x.Token.Equals(newRefreshToken.Token)))
            {
                return false;
            }
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            return true;
        }

        public async Task<bool> RemoveRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken == null)
            {
                return false;
            }

            _context.RefreshTokens.Remove(refreshToken);
            //await _context.SaveChangesAsync();

            return true;
        }
    }
}
