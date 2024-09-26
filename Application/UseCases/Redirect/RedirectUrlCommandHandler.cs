using Application.UseCases.Payment.Queries.GetOrderStatus;
using MediatR;

namespace Application.UseCases.Redirect
{
    public class RedirectUrlCommandHandler : IRequestHandler<RedirectUrlCommand, string>
    {
        private readonly IMediator _mediator;

        public RedirectUrlCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(RedirectUrlCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetOrderStatusQuery(request.appstransid), cancellationToken);
            return request.url!;
        }
    }
}
