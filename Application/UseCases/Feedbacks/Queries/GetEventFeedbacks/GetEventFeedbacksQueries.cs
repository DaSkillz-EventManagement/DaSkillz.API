using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Feedbacks.Queries.GetEventFeedbacks;

public class GetEventFeedbacksQueries: IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public int? NumberOfStar {  get; set; }
    public int Page {  get; set; }
    public int EachPage { get; set; }
    public Guid UserId { get; set; }
    public GetEventFeedbacksQueries(Guid eventId, int? numberOfStar, int page, int eachPage, Guid userId)
    {
        EventId = eventId;
        NumberOfStar = numberOfStar;
        Page = page;
        EachPage = eachPage;
        UserId = userId;
    }
}
