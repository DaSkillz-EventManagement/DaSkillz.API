using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequests
{
    public class GetSponsorRequestsQueries: IRequest<PagedList<SponsorEvent>?>
    {
        public Guid UserId { get; set; }
        public string? Status { get; set; }
        public int Page {  get; set; }
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
