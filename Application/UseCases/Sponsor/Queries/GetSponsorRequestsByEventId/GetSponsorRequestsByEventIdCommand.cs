using Domain.DTOs.Sponsors;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId
{
    public class GetSponsorRequestsByEventIdCommand: IRequest<PagedList<SponsorEventDto>>
    {
        public SponsorEventFilterDto SponsorFilter { get; set; }
        public Guid UserId { get; set; }
        public GetSponsorRequestsByEventIdCommand(SponsorEventFilterDto sponsorFilter, Guid userId)
        {
            SponsorFilter = sponsorFilter;
            UserId = userId;
        }
    }
}
