using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries.GetAccountStatistic
{
    public class GetAccountStatisticQuery : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }

        public GetAccountStatisticQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
