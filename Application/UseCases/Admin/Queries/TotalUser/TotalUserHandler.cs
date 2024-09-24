using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Queries.EventMonthly;

public class TotalUserHandler : IRequestHandler<TotalUserQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;

    public TotalUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(TotalUserQuery request, CancellationToken cancellationToken)
    {
        int totalUser = await _userRepository.GetTotalUsersAsync();
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = new
            {
                Total = totalUser
            }
        };
    }
}
