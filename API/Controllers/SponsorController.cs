﻿using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Sponsor.Commands.CreateSponsorRequest;
using Application.UseCases.Sponsor.Commands.DeleteSponsorRequest;
using Application.UseCases.Sponsor.Commands.UpdateSponsorRequest;
using Application.UseCases.Sponsor.Queries.GetSponsorRequestDetail;
using Application.UseCases.Sponsor.Queries.GetSponsorRequests;
using Application.UseCases.Sponsor.Queries.GetSponsorRequestsByEventId;
using Domain.DTOs.Sponsors;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
    public async Task<IActionResult> CreateRequest([FromBody] SponsorDto sponsorEvent, CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken();
        var result = await _mediator.Send(new CreateSponsorRequestCommand(sponsorEvent, Guid.Parse(userId)), token);
        if(result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [Authorize]
    [HttpPut("request-status")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRequest([FromBody] SponsorRequestUpdateDto sponsorRequestUpdate, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new UpdateSponsorRequestCommand(sponsorRequestUpdate, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [Authorize]
    [HttpGet("requested-sponsor")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    //Get requested-sponsor of this person
    public async Task<IActionResult> GetRequestSponsor(string? status, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10,
                                                        CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestsQueries(userId, status, pageNo, elementEachPage), token);
        if (result == null)
        {
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = null
            });
        }
        return Ok(new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        });
    }

    [Authorize]
    [HttpGet("event-filter")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    //Get requested-sponsor of this event
    public async Task<IActionResult> GetSponsorEvent([FromQuery] SponsorEventFilterDto sponsorFilter, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestsByEventIdQueries(sponsorFilter, userId), token);
        if (result == null)
        {
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = result
            });
        }
        return Ok(new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        });
    }
    [Authorize]
    [HttpGet("requested-detail")]
    //Get requested-sponsor of this person
    public async Task<IActionResult> GetRequestDetail(Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetSponsorRequestDetailQueries(eventId, userId), token);
        return Ok(result);
    }
    [Authorize]
    [HttpDelete("request")]
    public async Task<IActionResult> DeleteRequest(Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new DeleteSponsorRequestCommand(eventId, userId), token);
        return Ok(result);
    }   
}
