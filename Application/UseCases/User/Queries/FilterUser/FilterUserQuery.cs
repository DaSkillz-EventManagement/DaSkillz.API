using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.User.Queries.FilterUser
{
    public class FilterUserQuery : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Avatar { get; set; }

        public FilterUserQuery(Guid userId, string? fullName, string? email, string? phone, DateTime? updatedAt, DateTime? createdAt, string status, int roleId, string? avatar)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            Phone = phone;
            UpdatedAt = updatedAt;
            CreatedAt = createdAt;
            Status = status;
            RoleId = roleId;
            Avatar = avatar;
        }
    }
}
