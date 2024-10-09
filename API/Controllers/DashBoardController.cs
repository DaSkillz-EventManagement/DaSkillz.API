using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Admin.Queries.EventMonthly;
using Application.UseCases.Admin.Queries.EventStatus;
using Application.UseCases.Admin.Queries.GetTopSearch;
using Application.UseCases.Admin.Queries.GetTotalParticipant;
using Application.UseCases.Admin.Queries.GetTotalTransactionByDate;
using Application.UseCases.Admin.Queries.MonthlyEvent;
using Application.UseCases.Admin.Queries.MonthlyUser;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers;

//[Authorize(Roles = "Admin")]
[Route("api/v1/admin")]
[ApiController]
public class DashboardController : ControllerBase
{

    private ISender _mediator;


    public DashboardController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalCountableUser(CancellationToken token = default)
    {
        var result = await _mediator.Send(new TotalUserQuery(), token);
        return Ok(result);
    }

    [HttpGet("users/monthly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalUserByYear([FromQuery, Required] int year, CancellationToken token = default)
    {
        var response = await _mediator.Send(new MonthlyUserQuery(year), token);
        return Ok(response);
    }

    [HttpGet("event/status")]
    public async Task<IActionResult> CountByStatus(CancellationToken token = default)
    {
        var response = await _mediator.Send(new EventStatusQuery(), token);
        return Ok(response);
    }

    [HttpGet("event/monthly")]
    public async Task<IActionResult> CountEventsPerMonth([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, CancellationToken token = default)
    {
        var response = await _mediator.Send(new MonthlyEventQuery(startDate, endDate), token);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("total-participants")]
    [SwaggerOperation(Summary = "Get daily participant", Description = "with time: 2024-10-01T18:00 (without time will get 24 hours)")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalParticipants([FromQuery] Guid? eventId, [FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate, [FromQuery] bool isDay, CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetTotalParticipantByDateQuery(userId, eventId, startDate, endDate, isDay), cancellationToken);
        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }

    [Authorize]
    [HttpGet("total-transactions")]
    [SwaggerOperation(Summary = "Get daily transactions", Description = "with time: 2024-10-01T18:00 (without time will get 24 hours) //  Type: 1=Ticket, 2=Sponsor, 3=Advertise, 4=Subscription.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalTransactions(
        [FromQuery] Guid? eventId, 
        [FromQuery, Required] DateTime startDate, 
        [FromQuery, Required] DateTime endDate, 
        [FromQuery] bool isDay, 
        [FromQuery, Required] int TransactionType, 
        CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetTotalTransactionByDate(userId, eventId, startDate, endDate, isDay, TransactionType), cancellationToken);

        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }

    [Authorize]
    [HttpGet("total-type-transactions")]
    [SwaggerOperation(Summary = "Get all type of daily transactions", Description = "with time: 2024-10-01T18:00 (without time will get 24 hours) //  Type: 1=Ticket, 2=Sponsor, 3=Advertise, 4=Subscription.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalTypeTransactions(
        [FromQuery] Guid? eventId,
        [FromQuery, Required] DateTime startDate,
        [FromQuery, Required] DateTime endDate,
        [FromQuery] bool isDay,
        CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetTotalTransactionByDate(userId, eventId, startDate, endDate, isDay, null), cancellationToken);

        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }

    [HttpGet("top-keyword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSearchHistory([FromQuery, Required] int size, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTopSearchQuery(size),cancellationToken);
        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }
}
