using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Commands.Refund
{
    public class RefundCommand : IRequest<APIResponse>
    {
        public string? AppTransId { get; set; }
        public string? description { get; set; }
    }
}
