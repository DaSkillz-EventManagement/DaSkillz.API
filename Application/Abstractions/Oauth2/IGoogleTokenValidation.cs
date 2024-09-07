using Domain.Models.Response;

namespace Application.Abstractions.Oauth2
{
    public interface IGoogleTokenValidation
    {
        Task<APIResponse> ValidateGoogleTokenAsync(string token);
    }
}
