using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetOrderStatus
{
    public class GetOrderStatusQuery : IRequest<APIResponse>
    {
        public string? appTransId { get; set; }

        public GetOrderStatusQuery(string? appTransId)
        {
            this.appTransId = appTransId;
        }
    }
}
