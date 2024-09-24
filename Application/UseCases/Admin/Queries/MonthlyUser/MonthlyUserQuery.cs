using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.MonthlyUser;

public class MonthlyUserQuery: IRequest<APIResponse>
{
    public int Year {  get; set; }
    public MonthlyUserQuery(int year)
    {
        Year = year;
    }
}
