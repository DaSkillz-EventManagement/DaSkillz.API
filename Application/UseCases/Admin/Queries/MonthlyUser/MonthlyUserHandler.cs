using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.MonthlyUser;

public class MonthlyUserHandler : IRequestHandler<MonthlyUserQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;

    public MonthlyUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(MonthlyUserQuery request, CancellationToken cancellationToken)
    {
        var usersGroupedByMonth = await _userRepository.GetUsersCreatedInMonthAsync(request.Year);
        var monthlyUserCounts = usersGroupedByMonth.Select(g => new
        {
            Month = g.Key,
            Total = g.Count()
        }).ToList();

        var allMonths = Enumerable.Range(1, 12).Select(i => new
        {
            Month = i,
            Total = 0
        }).ToList();

        var result = allMonths
            .GroupJoin(monthlyUserCounts,
                       m => m.Month,
                       u => u.Month,
                       (m, u) => new { Month = m.Month, Total = u.Sum(x => x.Total) })
            .Select(x => new
            {
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month),
                Total = x.Total
            })
            .ToList();

        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = result
        };
    }
}
