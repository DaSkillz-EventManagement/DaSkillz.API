using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<APIResponse>
    {
        public Guid Id { get; set; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
