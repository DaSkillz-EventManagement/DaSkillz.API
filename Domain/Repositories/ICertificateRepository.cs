using Domain.Entities;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<List<Certificate>> GetFilteredCertificates(Guid? userId, Guid? eventId, DateTime? issueDate);
        Task<bool> CheckIfUserHaveCertificate(Guid userId, Guid eventId);
    }
}
