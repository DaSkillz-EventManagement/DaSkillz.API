using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.FilterUser
{
    public class FilterUserQueryCommand : IRequestHandler<FilterUserQuery, APIResponse>
    {
        public Task<APIResponse> Handle(FilterUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
