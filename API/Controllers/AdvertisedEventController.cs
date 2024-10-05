using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.AdvertiseEvents.Command.UseAdvertisedEvent;
using Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedEventByCreatedDay;
using Application.UseCases.AdvertiseEvents.Queries.GetAdvertisedInfoByEvent;
using Application.UseCases.AdvertiseEvents.Queries.GetFilteredAdveetisedByHost;
using Application.UseCases.Coupons.Queries.GetCoupon;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
        public async Task<ActionResult<APIResponse>> AddAdvertised([FromBody] Guid eventId, int numOfDate, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            
            var result = await _mediator.Send(new UseAdvertisedEventQuery(eventId, userId, numOfDate), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAdvertisedEvents(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAdvertisedEventQuery(), cancellationToken);

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }

        [Authorize]
        [HttpGet("ad-info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAdvertisedInfoByEvent([FromQuery] Guid eventId, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new GetAdvertisedInfoByEventQuery(eventId, userId), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }

        [Authorize]
        [HttpGet("ad-host")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAdvertisedInfoByEvent([FromQuery] string status, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new GetFilteredAdvertisedByHostQuery(userId, status), cancellationToken);

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }
    }
}
