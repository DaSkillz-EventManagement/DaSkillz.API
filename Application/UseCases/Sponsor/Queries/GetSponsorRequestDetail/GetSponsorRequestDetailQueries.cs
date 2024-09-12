using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestDetail;

public class GetSponsorRequestDetailQueries: IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public GetSponsorRequestDetailQueries(Guid eventId, Guid userId) 
    {
        EventId = eventId;
        UserId = userId;
    }
}
