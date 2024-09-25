using Domain.DTOs.Sponsors;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId
{
    public class GetSponsorRequestsByEventIdQueries : IRequest<PagedList<SponsorEventDto>>
    {
        public SponsorEventFilterDto SponsorFilter { get; set; }
        public Guid UserId { get; set; }
        public GetSponsorRequestsByEventIdQueries(SponsorEventFilterDto sponsorFilter, Guid userId)
        {
            SponsorFilter = sponsorFilter;
            UserId = userId;
        }
    }
}
