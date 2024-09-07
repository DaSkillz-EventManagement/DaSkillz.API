using Application.ResponseMessage;
using Application.UseCases.Events.Command.CreateEvent;
using Application.UseCases.Events.Command.GetEvent;
using Application.UseCases.Events.Command.GetEventByTag;
using Application.UseCases.Events.Command.GetEventByUserRole;
using Application.UseCases.Events.Command.GetEventParticipatedByUser;
using Application.UseCases.Events.Command.GetFilteredEvent;
using Azure;
using Domain.Enum.Events;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private ISender _mediator;

        public EventController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("info")]
        public async Task<ActionResult<APIResponse>> GetEventInfo([FromQuery, Required] GetEventInfoCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [Authorize]
        [HttpGet("user-event-role")]
        public async Task<ActionResult<APIResponse>> GetEventByUserRole([FromQuery, Required] GetEventByUserRoleCommand command, CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken();
            var result = await _mediator.Send(command, cancellationToken);
            Response.Headers.Add("X-Total-Element", result.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", result.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", result.CurrentPage.ToString());
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.GetAllEvent,
                Data = result
            });
        }

        [HttpGet("tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetEventsByTag([FromQuery] GetEventByTagCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);
            if (response.TotalItems > 0)
            {
                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageEvent.GetAllEvent,
                    Data = response
                });
            }
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            });
        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetFilteredEventCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);
            if (response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageEvent.GetAllEvent,
                    Data = response
                });
            }
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            });
        }

        [Authorize]
        [HttpGet("user-participated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserParticipatedEvents([FromQuery] GetEventParticipatedByUserCommand command, CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken();
            var response = await _mediator.Send(command, cancellationToken);
            if (response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageEvent.UserParticipatedEvent,
                    Data = response
                });
            }
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            });
        }

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken();
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}
