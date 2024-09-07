using Application.Abstractions.Caching;
using Infrastructure.ExternalServices.Caching.Setting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices.Caching
{
    public class RedisCaching : IRedisCaching
    {
        //
        private readonly IDistributedCache _distributedCache;
        private readonly RedisSetting _redisSetting;

        public RedisCaching(IOptions<RedisSetting> config, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _redisSetting = config.Value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cacheValue = await _distributedCache.GetStringAsync(key);

            // If cache has value, return cache data
            if (!string.IsNullOrEmpty(cacheValue))
            {
                return JsonConvert.DeserializeObject<T>(cacheValue)!;
            }

            return default!;
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, double TimeToLive)
        {
            var cacheOpt = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(TimeToLive)
            };

            var jsonOpt = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value, jsonOpt), cacheOpt);

        }
    }
}
