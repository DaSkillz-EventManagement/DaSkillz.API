using Application.ResponseMessage;
using Application.UseCases.Admin.Queries.EventMonthly;
using Application.UseCases.Admin.Queries.EventStatus;
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

    [Authorize(Roles = "Admin")]
    [HttpGet("total-participants")]
    [SwaggerOperation(Summary = "Get daily participant", Description = "with time: 2024-10-01T18:00 without time will get 24 hours)")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalParticipants([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] bool isDay, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTotalParticipantByDateQuery(startDate, endDate, isDay), cancellationToken);
        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("total-transactions")]
    [SwaggerOperation(Summary = "Get daily transactions", Description = "with time: 2024-10-01T18:00 without time will get 24 hours)")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTotalTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] bool isDay, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTotalTransactionByDate(startDate, endDate, isDay), cancellationToken);

        return result.StatusResponse != System.Net.HttpStatusCode.OK
            ? StatusCode((int)result.StatusResponse, result)
            : Ok(result);
    }
}
