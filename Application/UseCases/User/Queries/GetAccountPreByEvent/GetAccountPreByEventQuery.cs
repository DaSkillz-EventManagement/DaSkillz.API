using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries.GetAccountPreByEvent
{
    public class GetAccountPreByEventQuery : IRequest<APIResponse>
    {
        public Guid AnotherUserId { get; set; }
        public Guid UserId { get; set; }

        public GetAccountPreByEventQuery(Guid anotherUserId, Guid userId)
        {
            AnotherUserId = anotherUserId;
            UserId = userId;
        }
    }
}
