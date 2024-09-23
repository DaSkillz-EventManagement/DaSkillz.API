using Domain.DTOs.Events.RequestDto;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.UploadEventSponsorLogo
{
    public class UploadEventSponsorLogoCommand : IRequest<string?>
    {
        public FileUploadDto FileUploadDto {  get; set; }

        public UploadEventSponsorLogoCommand(FileUploadDto fileUploadDto)
        {
            FileUploadDto = fileUploadDto;
        }
    }
}
