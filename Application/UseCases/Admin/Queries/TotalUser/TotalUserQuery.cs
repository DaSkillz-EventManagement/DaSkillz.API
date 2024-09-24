using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.EventMonthly;

public class TotalUserQuery: IRequest<APIResponse>
{
    public TotalUserQuery() { }
}
