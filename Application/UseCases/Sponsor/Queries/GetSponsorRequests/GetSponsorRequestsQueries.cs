using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequests
{
    public class GetSponsorRequestsQueries: IRequest<PagedList<SponsorEventDto>?>
    {
        public Guid UserId { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; }
        public int EachPage { get; set; }
        public GetSponsorRequestsQueries(Guid userId, string? status, int page, int eachPage)
        {
            UserId = userId;
            Status = status;
            Page = page;
            EachPage = eachPage;
        }
    }
}
