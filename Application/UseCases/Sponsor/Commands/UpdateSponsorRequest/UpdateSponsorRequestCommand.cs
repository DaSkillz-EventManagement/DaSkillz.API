using Domain.DTOs.Sponsors;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Sponsor.Commands.UpdateSponsorRequest;

public class UpdateSponsorRequestCommand : IRequest<APIResponse>
{
    public SponsorRequestUpdateDto SponsorRequestUpdateDto { get; set; }
    public Guid UserId { get; set; }
    public UpdateSponsorRequestCommand(SponsorRequestUpdateDto sponsorRequestUpdateDto, Guid userId)
    {
        SponsorRequestUpdateDto = sponsorRequestUpdateDto;
        UserId = userId;
    }
}
