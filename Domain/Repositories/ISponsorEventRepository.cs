using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories.Generic;
namespace Domain.Repositories
{
    public interface ISponsorEventRepository : IRepository<SponsorEvent>
    {
        Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId);
        Task<SponsorEvent?> CheckSponsoredEvent(Guid eventId, Guid userId);
        Task<PagedList<SponsorEvent>> GetSponsorEvents(SponsorEventFilterDto sponsorFilter);
        Task<PagedList<SponsorEvent>> GetRequestSponsor(Guid userId, string? status, int page, int eachPage);
        Task<SponsorEvent?> DeleteSponsorRequest(Guid eventId, Guid userId);
        Task<bool> IsSponsored(Guid eventId);
    }
}
