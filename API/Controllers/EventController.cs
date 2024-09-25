using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Events.Command.CreateEvent;
using Application.UseCases.Events.Command.DeleteEvent;
using Application.UseCases.Events.Command.UpdateEvent;
using Application.UseCases.Events.Command.UploadEventSponsorLogo;
using Application.UseCases.Events.Queries.GetAllBlobUris;
using Application.UseCases.Events.Queries.GetBlobUri;
using Application.UseCases.Events.Queries.GetEventByTag;
using Application.UseCases.Events.Queries.GetEventByUserRole;
using Application.UseCases.Events.Queries.GetEventInfo;
using Application.UseCases.Events.Queries.GetEventParticipatedByUser;
using Application.UseCases.Events.Queries.GetFilteredEvent;
using Application.UseCases.Events.Queries.GetTopCreatorsByEventCount;
using Application.UseCases.Events.Queries.GetTopLocationByEventCount;
using Application.UseCases.Events.Queries.GetUserHostEvent;
using Application.UseCases.Events.Queries.GetUserPastAndIncomingEvent;
using Domain.DTOs.Events;
using Domain.DTOs.Events.RequestDto;
using Domain.Enum.Events;
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
        public async Task<ActionResult<APIResponse>> GetEventInfo([FromQuery, Required] Guid eventId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetEventInfoQuery(eventId), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [Authorize]
        [HttpGet("user-event-role")]
        public async Task<ActionResult<APIResponse>> GetEventByUserRole([FromQuery, Required] EventRole eventRole,
                                                        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());

            var result = await _mediator.Send(new GetEventByUserRoleQuery(eventRole, userId, pageNo, elementEachPage), cancellationToken);
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
        public async Task<ActionResult<APIResponse>> GetEventsByTag([FromQuery] List<int> TagId,
                                                        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetEventByTagQuery(TagId, pageNo, elementEachPage), cancellationToken);
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
        public async Task<ActionResult<APIResponse>> GetAllEvents([FromQuery] EventFilterObjectDto filterObject,
                                                      [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                      [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetFilteredEventQuery(filterObject, pageNo, elementEachPage), cancellationToken);

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

        [Authorize]
        [HttpGet("user-participated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUserParticipatedEvents([FromQuery] EventFilterObjectDto filter,
                                                                   [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());

            var response = await _mediator.Send(new GetEventParticipatedByUserQuery(filter, userId, pageNo, elementEachPage), cancellationToken);
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
        [HttpGet("user-past-and-incoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserPastAndIncomingEvent(CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var response = await _mediator.Send(new GetUserPastAndIncomingEventQuery(userId), cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response
            });
        }


        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddEvent(EventRequestDto eventRequestDto, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());

            var result = await _mediator.Send(new CreateEventCommand(eventRequestDto, userId), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }



        [Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateEvent([FromBody] EventRequestDto eventRequestDto, [FromQuery, Required] Guid eventId, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());

            var result = await _mediator.Send(new UpdateEventCommand(eventRequestDto, userId, eventId), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [Authorize]
        [HttpDelete("")]
        public async Task<ActionResult<APIResponse>> DeleteEvent([FromQuery, Required] Guid eventId, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());

            APIResponse response = new APIResponse();
            var result = await _mediator.Send(new DeleteEventCommand(eventId, userId), cancellationToken);
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
        public async Task<ActionResult<APIResponse>> UploadEventLogoImage([FromBody] FileUploadDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new UploadEventSponsorLogoCommand(dto), cancellationToken);
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
        public async Task<ActionResult<APIResponse>> GetBlobUri([FromQuery] string brandName, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetBlobUriQuery(brandName), cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }

        [HttpGet("/logo/all")]
        public async Task<ActionResult<APIResponse>> GetAllLogos(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllBlobUrisQuery(), cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }

        [HttpGet("/logo/event-logo")]
        public async Task<ActionResult> GetAllEventBlobUri([FromQuery] Guid eventId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllBlobUrisQuery(), cancellationToken);
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
        public async Task<ActionResult> PopularOrganizers(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetTopCreatorsByEventQuery(), cancellationToken);
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
        public async Task<ActionResult> Locations(CancellationToken cancellationToken = default)
        {
            var result = _mediator.Send(new GetTopLocationByEventQuery(), cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.PopularLocation,
                Data = result
            });
        }

        [HttpGet("user-hosted")]
        public async Task<ActionResult> GetUserHostEvent([FromQuery, Required] Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetUserHostEventQuery(userId), cancellationToken);
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
