using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.MonthlyEvent;

public class MonthlyEventQuery: IRequest<APIResponse>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MonthlyEventQuery(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}
