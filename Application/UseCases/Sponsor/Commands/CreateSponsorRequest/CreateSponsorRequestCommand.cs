using Domain.DTOs.Sponsors;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sponsor.Commands.CreateSponsorRequest
{
    public class CreateSponsorRequestCommand : IRequest<APIResponse>
    {
        public SponsorDto Sponsor { get; set; }
        public Guid UserId { get; set; }
        public CreateSponsorRequestCommand(SponsorDto sponsor, Guid userId)
        {
            Sponsor = sponsor;
            UserId = userId;
        }
    }
}
