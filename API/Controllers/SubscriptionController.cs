using Application.UseCases.Payment.Queries.GetFilterTransaction;
using Application.UseCases.Subscriptions.Query;
using Domain.Enum.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/subcriptions")]
    [ApiController]
    public class SubscriptionController : Controller
    {
        private readonly IMediator _mediator;

        public SubscriptionController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet("filter")]
        [SwaggerOperation(Summary = "Get filtered subcriptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFilterTransaction(
                                                [FromQuery] Guid? userId,
                                                [FromQuery] bool? isActive,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new FIilterSubscriptionQuery(userId, isActive), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }
    }
}
