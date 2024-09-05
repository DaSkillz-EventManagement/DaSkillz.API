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

    //[HttpGet("id")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> GetUserById([FromQuery, Required] Guid userId)
    //{
    //    var result = await _mediator.GetUserByIdAsync(userId);
    //    return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
    //}

    //[HttpGet("all")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> GetAllUsers([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10)
    //{
    //    var result = await _mediator.GetAllUser(pageNo, eachPage);
    //    return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
    //}

    //[Authorize]
    //[HttpGet("keyword")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> GetUserByKeyword([FromQuery] string keyword)
    //{
    //    var result = await _mediator.GetByKeyWord(keyword);
    //    return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
    //}

    //[Authorize]
    //[HttpPut("update")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> Update(UpdateDeleteUserDto userDto)
    //{
    //    var result = await _userService.UpdateUser(userDto);
    //    return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
    //}

    //[Authorize("Admin")]
    //[HttpDelete("delete")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> Delete([FromQuery] Guid userId)
    //{
    //    var result = await _userService.DeleteUser(userId);
    //    return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
    //}

}
