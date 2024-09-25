using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePayment : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string Amount { get; set; }
        public Guid? EventId { get; set; }
        public int SubscriptionType { get; set; }
        public string? redirectUrl { get; set; }
    }
}
