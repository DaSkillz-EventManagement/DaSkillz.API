using Application.Abstractions.Oauth2;
using Domain.Models.Oauth2;
using Domain.Models.Response;
using Infrastructure.ExternalServices.Oauth2.Setting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace Infrastructure.ExternalServices.Oauth2
{
    public class GoogleTokenValidation : IGoogleTokenValidation
    {
        private readonly GoogleSetting _setting;

        public GoogleTokenValidation(IOptions<GoogleSetting> setting)
        {
            _setting = setting.Value;
        }

        public async Task<APIResponse> ValidateGoogleTokenAsync(string token)
        {

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_setting.Url}{token}");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.Unauthorized,
                        Message = "Invalid Google token",
                        Data = null
                    };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfo>(responseString);

                if (tokenInfo == null || tokenInfo.Audience != _setting.Audience)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.Unauthorized,
                        Message = "Invalid Google token",
                        Data = null
                    };
                }

                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "Valid Google token",
                    Data = tokenInfo.Email,
                };
            }
        }
    }




}
