using Domain.DTOs.Events.RequestDto;
using MediatR;

namespace Application.UseCases.Events.Command.UploadEventSponsorLogo
{
    public class UploadEventSponsorLogoCommand : IRequest<string?>
    {
        public FileUploadDto FileUploadDto { get; set; }

        public UploadEventSponsorLogoCommand(FileUploadDto fileUploadDto)
        {
            FileUploadDto = fileUploadDto;
        }
    }
}
