using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Certificate.Queries.GetFilterCertificate
{
    public class GetFilterCertificate : IRequest<APIResponse>
    {
        public Guid? certificateId { get; set; }
        public Guid? userId { get; set; }
        public Guid? eventId { get; set; }
        public DateTime? issueDate { get; set; }

        public GetFilterCertificate(Guid? certificateId, Guid? userId, Guid? eventId, DateTime? issueDate)
        {
            this.certificateId = certificateId;
            this.userId = userId;
            this.eventId = eventId;
            this.issueDate = issueDate;
        }
    }
}
