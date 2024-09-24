using Domain.DTOs.Sponsors;
using MediatR;

namespace Application.UseCases.Events.Queries.GetBlobUri
{
    public class GetBlobUriQuery : IRequest<SponsorLogoDto>
    {
        public string BlobName { get; set; }

        public GetBlobUriQuery(string blobName)
        {
            BlobName = blobName;
        }
    }
}
