using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdEventStatistic
{
    public class GetAdEventStatisticQuery : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }

        public GetAdEventStatisticQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
