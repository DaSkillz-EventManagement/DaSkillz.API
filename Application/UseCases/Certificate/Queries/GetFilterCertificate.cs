using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Certificate.Queries
{
    public class GetFilterCertificate : IRequest<APIResponse>
    {
        public Guid? userId { get; set; }
        public Guid? eventId { get; set; }
        public DateTime? issueDate { get; set; }

        public GetFilterCertificate(Guid? userId, Guid? eventId, DateTime? issueDate)
        {
            this.userId = userId;
            this.eventId = eventId;
            this.issueDate = issueDate;
        }
    }
}
