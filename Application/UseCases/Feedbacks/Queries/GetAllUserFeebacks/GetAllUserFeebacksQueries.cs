using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Feedbacks.Queries.GetAllUserFeebacks
{
    public class GetAllUserFeebacksQueries : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int EachPage { get; set; }
        public GetAllUserFeebacksQueries(Guid userId, int page, int eachPage)
        {
            UserId = userId;
            Page = page;
            EachPage = eachPage;
        }
    }
}
