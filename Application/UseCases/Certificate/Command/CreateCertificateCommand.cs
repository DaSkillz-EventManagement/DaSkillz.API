using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Certificate.Command
{
    public class CreateCertificateCommand : IRequest<APIResponse>
    {
        public Guid EventId { get; set; }    
        public List<Guid> UserIds { get; set; }   

        public CreateCertificateCommand(Guid eventId, List<Guid> userIds)
        {
            EventId = eventId;
            UserIds = userIds;
        }
    }
}
