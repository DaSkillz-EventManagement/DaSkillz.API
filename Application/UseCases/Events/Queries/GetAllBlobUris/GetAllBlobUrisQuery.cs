using Domain.DTOs.Sponsors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetAllBlobUris
{
    public class GetAllBlobUrisQuery : IRequest<List<SponsorLogoDto>>
    {
    }
}
