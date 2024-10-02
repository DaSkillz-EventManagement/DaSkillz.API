using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task AddRange(IEnumerable<Certificate> certificates);
        Task<List<Certificate>> GetFilteredCertificates(Guid? userId, Guid? eventId, DateTime? issueDate);
        Task<bool> CheckIfUserHaveCertificate(Guid userId, Guid eventId);
    }
}
