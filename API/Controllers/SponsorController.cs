using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Sponsor.Commands.CreateSponsorRequest;
using Application.UseCases.Sponsor.Commands.DeleteSponsorRequest;
using Application.UseCases.Sponsor.Commands.UpdateSponsorRequest;
using Application.UseCases.Sponsor.Queries;
using Application.UseCases.Sponsor.Queries.GetSponsorRequestDetail;
using Application.UseCases.Sponsor.Queries.GetSponsorRequests;
using Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId;
using Azure;
using Domain.DTOs.Sponsors;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers;

[Route("api/v1/sponsor")]
[ApiController]
public class SponsorController : ControllerBase
{
    private ISender _mediator;
    public SponsorController(ISender mediator)
    {
        _mediator = mediator;
    }
    [Authorize]
    [HttpPost("request")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<APIResponse> CreateRequest([FromBody] SponsorDto sponsorEvent, CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken();
        var result = await _mediator.Send(new CreateSponsorRequestCommand(sponsorEvent, Guid.Parse(userId)), token);
        return result;
    }
    [Authorize]
    [HttpPut("request-status")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<APIResponse> UpdateRequest([FromBody] SponsorRequestUpdateDto sponsorRequestUpdate, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new UpdateSponsorRequestCommand(sponsorRequestUpdate, userId), token);
        return result;
    }
    [Authorize]
    [HttpGet("requested-sponsor")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    //Get requested-sponsor of this person
    public async Task<APIResponse> GetRequestSponsor(string? status, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10,
                                                        CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestsCommand(userId, status, pageNo, elementEachPage), token);
        if (result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = null
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        };
    }

    [Authorize]
    [HttpGet("event-filter")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    //Get requested-sponsor of this event
    public async Task<APIResponse> GetSponsorEvent([FromQuery] SponsorEventFilterDto sponsorFilter, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestsByEventIdCommand(sponsorFilter, userId), token);
        if (result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = result
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        };
    }
    [Authorize]
    [HttpGet("requested-detail")]
    //Get requested-sponsor of this person
    public async Task<APIResponse> GetRequestDetail(Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestDetailCommand(eventId, userId), token);
        return result;
    }
    [Authorize]
    [HttpDelete("request")]
    public async Task<APIResponse> DeleteRequest(Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new DeleteSponsorRequestCommand(eventId, userId), token);
        return result;
    }
}
