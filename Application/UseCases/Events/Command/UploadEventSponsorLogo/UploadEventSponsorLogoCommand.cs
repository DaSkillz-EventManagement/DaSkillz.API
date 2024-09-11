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
        public string base64 {  get; set; }
        public Guid EventId { get; set; }
        public string sponsorName {  get; set; }

        public UploadEventSponsorLogoCommand(string base64, Guid eventId, string sponsorName)
        {
            this.base64 = base64;
            EventId = eventId;
            this.sponsorName = sponsorName;
        }
    }
}
