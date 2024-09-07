using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<APIResponse>
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }

        public UpdateUserCommand(string? email, string? fullName, string? phone, string? avatar)
        {
            Email = email;
            FullName = fullName;
            Phone = phone;
            Avatar = avatar;
        }
    }
}
