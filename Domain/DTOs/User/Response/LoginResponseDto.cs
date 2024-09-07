using Domain.DTOs.AuthenticationDTO;

namespace Domain.DTOs.User.Response
{
    public class LoginResponseDto
    {
        public UserResponseDto? UserData { get; set; }
        public TokenResponseDTO? Token { get; set; }
    }
}
