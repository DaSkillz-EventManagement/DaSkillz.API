using Domain.DTOs.User.Response;
using Domain.Repositories;

namespace Application.Helper
{
    public static class EventHelper
    {
        public static IUserRepository _userRepo;
        public static CreatedByUserDto GetHostInfo(Guid userId)
        {
            var user = _userRepo.GetUserById(userId);
            CreatedByUserDto response = new CreatedByUserDto();
            response.avatar = user!.Avatar;
            response.Id = user.UserId;
            response.Name = user.FullName;
            return response;
        }
    }
}
