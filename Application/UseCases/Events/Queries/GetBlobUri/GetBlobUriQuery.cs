using Domain.DTOs.Sponsors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetBlobUri
{
    public class GetBlobUriQuery : IRequest<SponsorLogoDto>
    {
        public string blobName { get; set; }

        public GetBlobUriQuery(string blobName)
        {
            this.blobName = blobName;
        }
    }
}
