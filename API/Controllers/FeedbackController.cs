using Application.Helper;
using Application.UseCases.Feedbacks.Commands.CreateFeedback;
using Application.UseCases.Feedbacks.Commands.UpdateFeedback;
using Application.UseCases.Feedbacks.Queries.GetAllUserFeebacks;
using Application.UseCases.Feedbacks.Queries.GetEventFeedbacks;
using Application.UseCases.Feedbacks.Queries.GetUserFeedBack;
using Domain.DTOs.Feedbacks;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers;

[Route("api/v1/feedback")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private ISender _mediator;
    public FeedbackController(ISender mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateFeedback([FromBody] FeedbackDto feedbackDto, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new CreateFeedbackCommand(feedbackDto, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [Authorize]
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackDto feedbackDto, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new UpdateFeedbackCommand(feedbackDto, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [Authorize]
    [HttpGet("event")]

    //Get all feedback for my event
    public async Task<IActionResult> GetEventFeedbacks([FromQuery, Required] Guid eventId, [FromQuery, Range(1, 5)] int? numOfStar, [FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetEventFeedbacksQueries(eventId, numOfStar, page, eachPage, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        if (result.StatusResponse == HttpStatusCode.NotFound)
        {
            return NotFound(result);
        }
        return BadRequest(result);
    }

    [Authorize]
    [HttpGet("event/user")]
    //Gte feedback detail for specific event
    public async Task<IActionResult> GetUserFeedBack([FromQuery, Required] Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetUserFeedBackQueries(eventId, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [Authorize]
    [HttpGet("user")]
    //Get all feedback of this user for all events
    public async Task<IActionResult> GetAllUserFeebacks([FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetAllUserFeebacksQueries(userId, page, eachPage), token);
        return Ok(result);
    }

}
