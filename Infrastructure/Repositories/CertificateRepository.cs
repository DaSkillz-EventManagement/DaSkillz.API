using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(IEnumerable<Certificate> certificates)
        {
            await _context.Certificates.AddRangeAsync(certificates);        
        }

        public async Task<bool> CheckIfUserHaveCertificate(Guid userId, Guid eventId)
        {
            return await _context.Certificates.AnyAsync(a => a.UserId ==  userId && a.EventId == eventId);
        }

        public async Task<List<Participant>> GetFilteredCertificates(Guid? certificateId, Guid? userId, Guid? eventId, DateTime? issueDate)
        {
            var query = _context.Participants.AsQueryable();

            if (certificateId.HasValue)
            {
                query = query.Where(p => p.Certificates.Any(c => c.CertificateID == certificateId.Value));
            }

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }

            if (eventId.HasValue)
            {
                query = query.Where(p => p.EventId == eventId.Value);
            }

            if (issueDate.HasValue)
            {
                query = query.Where(p => p.Certificates.Any(c => c.IssueDate.Date == issueDate.Value.Date));
            }

            return await query.ToListAsync();
        }

    }
}
