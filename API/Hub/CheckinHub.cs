using Application.ResponseMessage;
using Domain.DTOs.Hub;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Text.Json;

namespace API.Hub;
public interface ICheckinHub
{
    Task SendNotification(SocketResponse socketResponse);

}
public class CheckinHub : Hub<ICheckinHub>
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckinHub(IParticipantRepository participantRepository, IUnitOfWork unitOfWork)
    {
        _participantRepository = participantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> CheckinUser(Guid userId, Guid eventId)
    {
        var participant = await _participantRepository.GetParticipant(userId, eventId);

        if (participant == null)
        {
            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageParticipant.CheckInUserFailed,
                Data = JsonSerializer.Serialize(new { userId, eventId })
            };
        }
        participant.CheckedIn = DateTime.Now;

        await _participantRepository.Update(participant);

        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageParticipant.CheckInUserSuccess,
                Data = JsonSerializer.Serialize(new { userId, eventId })
            };
        }
        return new APIResponse()
        {
            StatusResponse = HttpStatusCode.InternalServerError,
            Message = MessageCommon.ServerError,
            Data = JsonSerializer.Serialize(new { userId, eventId })
        };
    }
}

