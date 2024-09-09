using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Globalization;
using System.Net;

namespace Application.UseCases.User.Queries.GetTotalUserByYear
{
    public class GetTotalUserByYearHandler : IRequestHandler<GetTotalUserByYearQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetTotalUserByYearHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<APIResponse> Handle(GetTotalUserByYearQuery request, CancellationToken cancellationToken)
        {
            var usersGroupedByMonth = await _userRepository.GetUsersCreatedInMonthAsync(request.key);

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
}
