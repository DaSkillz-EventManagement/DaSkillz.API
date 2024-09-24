using Domain.DTOs.Sponsors;
using MediatR;

namespace Application.UseCases.Events.Queries.GetAllBlobUris
{
    public class GetAllBlobUrisQuery : IRequest<List<SponsorLogoDto>>
    {
    }
}
