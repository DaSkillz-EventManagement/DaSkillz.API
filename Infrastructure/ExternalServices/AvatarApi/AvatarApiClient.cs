
using Application.Abstractions.AvatarApi;
using Infrastructure.ExternalServices.AvatarApi.Setting;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices.AvatarApi
{

    public class AvatarApiClient : IAvatarApiClient
    {
        private readonly AvatarApiSetting _setting;

        public AvatarApiClient(IOptions<AvatarApiSetting> setting)
        {
            _setting = setting.Value;
        }

        public string GetRandomAvatarUrl()
        {
            return $"{_setting.Url}/public";
        }

        public string GetRandomBoyAvatarUrl()
        {
            return $"{_setting.Url}/public/boy";
        }

        public string GetRandomGirlAvatarUrl()
        {
            return $"{_setting.Url}/public/girl";
        }

        public string GetAvatarUrlWithName(string fullName)
        {
            return $"{_setting.Url}/username?username={fullName}";
        }
    }
}
