using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Participants.Commands.AddUserToEventCommand;
using Application.UseCases.Participants.Commands.ApproveInvite;
using Application.UseCases.Participants.Commands.CheckinParticipant;
using Application.UseCases.Participants.Commands.DeleteParticipantCommand;
using Application.UseCases.Participants.Commands.ProcessTicketParticipant;
using Application.UseCases.Participants.Commands.RegisterEventCommand;
using Application.UseCases.Participants.Commands.UpdateRoleEventCommand;
using Application.UseCases.Participants.Queries.GetCurrentUser;
using Application.UseCases.Participants.Queries.GetParticipantOnEvent;
using Application.UseCases.Participants.Queries.GetParticipantRelatedToCheckInOnEvent;
using Domain.DTOs.ParticipantDto;
using Domain.Enum.Participant;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers;

[Route("api/v1/participants")]
[ApiController]
public class ParticipantController : ControllerBase
{
    private ISender _mediator;
    public ParticipantController(ISender mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterEvent([FromQuery, Required] Guid eventId, Guid? transactionId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new RegisterEventCommand(userId, transactionId, eventId), token);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> AddParticipantToEvent(RegisterEventModel registerEventModel, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new AddUserToEventCommand(registerEventModel, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpDelete("")]
    [Authorize]
    public async Task<IActionResult> DeleteParticipant(Guid userId, Guid eventId, CancellationToken token = default)
    {
        Guid executor = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new DeleteParticipantCommand(userId, eventId, executor), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        { return Ok(result); }
        return BadRequest(result);

    }
    [HttpPut("role")]
    [Authorize]
    public async Task<IActionResult> UpdateRoleEvent(RegisterEventModel registerEventModel, CancellationToken token = default)
    {
        Guid executor = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new UpdateRoleEventCommand(registerEventModel, executor), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetParticipantOnEvent([FromQuery] FilterParticipantDto filter, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetParticipantOnEventQueries(filter, userId), token);
        if (result != null)
        {
            Response.Headers.Add("X-Total-Element", result.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", result.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", result.CurrentPage.ToString());
            return Ok(result);
        }
        return BadRequest(new APIResponse()
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageParticipant.NotOwner,
            Data = null
        });
    }
    [HttpGet("checkin")]
    [Authorize]
    public async Task<IActionResult> GetParticipantRelatedToCheckInOnEvent([Required] Guid eventId, int page = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetParticipantRelatedToCheckInOnEventQueries(eventId, page, eachPage, userId), token);
        if (result != null)
        {
            Response.Headers.Add("X-Total-Element", result.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", result.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", result.CurrentPage.ToString());
            return Ok(result);
        }
        return BadRequest(new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageParticipant.NotOwner,
            Data = null
        });
    }
    [HttpPost("process-ticket")]
    [Authorize]
    public async Task<IActionResult> ProcessTicketParticipant(ParticipantTicket participantTicket, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new ProcessTicketParticipantCommand(participantTicket, userId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("qrcode")]
    public IActionResult QRCode(Guid eventId, Guid userId)
    {
        var bytes = QRCodeHelper.GenerateQRCode(userId.ToString());

        return File(bytes, "image/png");
    }
    [HttpGet("approval/{eventId}")]
    [Authorize]
    public async Task<IActionResult> ApprovalInvite(Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new ApproveInviteCommand(eventId, userId, ParticipantStatus.Confirmed.ToString()), token);
        if (result.StatusResponse == HttpStatusCode.OK) { return Ok(result); }
        return BadRequest(result);
    }
    [Authorize]
    [HttpGet("current-user")]
    public async Task<IActionResult> GetUserRegisterStatus([Required] Guid eventId, CancellationToken token = default)
    {
        Guid userId = Guid.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetCurrentUserQueries(userId, eventId), token);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("checkin-user")]
    public async Task<IActionResult> CheckinParticipant(Guid eventId, Guid userId, CancellationToken token = default)
    {
        var result = await _mediator.Send(new CheckinParticipantCommand(userId, eventId), token);
        if (result.StatusResponse == HttpStatusCode.OK)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
