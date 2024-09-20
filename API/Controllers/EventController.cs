using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Events.Command.CreateEvent;
using Application.UseCases.Events.Command.DeleteEvent;
using Application.UseCases.Events.Command.UpdateEvent;
using Application.UseCases.Events.Command.UploadEventSponsorLogo;
using Application.UseCases.Events.Queries.GetAllEventBlobUris;
using Application.UseCases.Events.Queries.GetBlobUri;
using Application.UseCases.Events.Queries.GetEventByTag;
using Application.UseCases.Events.Queries.GetEventByUserRole;
using Application.UseCases.Events.Queries.GetEventInfo;
using Application.UseCases.Events.Queries.GetEventParticipatedByUser;
using Application.UseCases.Events.Queries.GetFilteredEvent;
using Application.UseCases.Events.Queries.GetTopCreatorsByEventCount;
using Application.UseCases.Events.Queries.GetTopLocationByEventCount;
using Application.UseCases.Events.Queries.GetUserHostEvent;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<APIResponse>> GetEventInfo([FromQuery, Required] GetEventInfoQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [Authorize]
        [HttpGet("user-event-role")]
        public async Task<ActionResult<APIResponse>> GetEventByUserRole([FromQuery] GetEventByUserRoleQuery command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.UserId = Guid.Parse(userId);
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
        public async Task<ActionResult<APIResponse>> GetEventsByTag([FromQuery] GetEventByTagQuery command, CancellationToken cancellationToken = default)
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
        public async Task<ActionResult<APIResponse>> GetAllEvents([FromQuery] GetFilteredEventQuery command, CancellationToken cancellationToken = default)
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
        public async Task<ActionResult<APIResponse>> GetUserParticipatedEvents([FromQuery] GetEventParticipatedByUserQuery command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.UserId = Guid.Parse(userId);
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

        //[Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.UserId = Guid.Parse(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }



        //[Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateEvent([FromBody] UpdateEventCommand command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.UserId = Guid.Parse(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [Authorize]
        [HttpDelete("")]
        public async Task<ActionResult<APIResponse>> DeleteEvent([FromQuery, Required] DeleteEventCommand command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.UserId = Guid.Parse(userId);
            APIResponse response = new APIResponse();
            var result = await _mediator.Send(command, cancellationToken);
            if (result)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.DeleteSuccessfully;
                response.Data = result;
                return Ok(response);
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.DeleteFailed;
                return BadRequest(response);
            }

        }

        [HttpPost("logo-upload")]
        public async Task<ActionResult<APIResponse>> UploadEventLogoImage([FromBody] UploadEventSponsorLogoCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result != null)
            {
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = result
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = "Logo already exist!"
            });

        }


        [HttpGet("/logo")]
        public async Task<ActionResult<APIResponse>> GetBlobUri([FromQuery] GetBlobUriQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }

        [HttpGet("/logo/all")]
        public async Task<ActionResult<APIResponse>> GetAllLogos([FromQuery] GetBlobUriQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }

        [HttpGet("/logo/event-logo")]
        public async Task<IActionResult> GetAllEventBlobUri([FromQuery] GetAllEventBlobUrisQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }

        [HttpGet("popular/organizers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PopularOrganizers([FromQuery] GetTopCreatorsByEventQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.PopularOrganizers,
                Data = result
            });
        }

        [HttpGet("popular/locations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Locations([FromQuery] GetTopLocationByEventQuery command, CancellationToken cancellationToken = default)
        {
            var result = _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.PopularLocation,
                Data = result
            });
        }

        [HttpGet("user-hosted")]
        public async Task<IActionResult> GetUserHostEvent([FromQuery, Required] GetUserHostEventQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result != null)
            {
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = result
                });
            }
            return NotFound(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound
            });
        }


    }
}
