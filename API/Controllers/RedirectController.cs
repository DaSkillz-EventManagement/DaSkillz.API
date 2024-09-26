using Application.UseCases.Payment.Queries.GetOrderStatus;
using Application.UseCases.Redirect;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/redirect")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RedirectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        [SwaggerOperation(Summary = "Redirect with url")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RedirectUrl([FromQuery] string url, [FromQuery] string apptransid,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new RedirectUrlCommand(url, apptransid), cancellationToken);
            return Redirect(result);
        }
    }
}
