using Application.UseCases.User.Commands.DeleteUser;
using Application.UseCases.User.Commands.UpdateUser;
using Application.UseCases.User.Queries.GetAllUsers;
using Application.UseCases.User.Queries.GetByUserId;
using Application.UseCases.User.Queries.GetUserByKeyword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : Controller
{
    private ISender _mediator;
    public UserController(ISender mediator)
    {
        _mediator = mediator;

    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllUsers([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllUserQuery(pageNo, eachPage), cancellationToken);
        return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    }

    [Authorize]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUser, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(updateUser, cancellationToken);
        return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    }

    //[Authorize("Admin")]
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);
        return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    }

    //[Authorize]
    [HttpGet("keyword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByKeyword([FromQuery] string keyword, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByKeyword(keyword), cancellationToken);
        return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    }

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserById([FromQuery, Required] Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserById(userId), cancellationToken);
        return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    }

    //[HttpGet("check-premium")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> CheckPremium(CancellationToken cancellationToken = default)
    //{
    //    var result = await _mediator.Send(new GetAllUserQuery(pageNo, eachPage), cancellationToken);
    //    return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
    //}
}
