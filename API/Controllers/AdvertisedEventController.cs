using Application.Helper;
using Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/advertised")]
    [ApiController]
    public class AdvertisedEventController : ControllerBase
    {
        private ISender _mediator;


        public AdvertisedEventController(ISender mediator)
        {
            _mediator = mediator;

        }

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddAdvertised([FromBody] UseAdvertisedEventQuery query, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            query.UserId = Guid.Parse(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}
