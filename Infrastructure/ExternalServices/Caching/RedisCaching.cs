using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.ExternalServices.Caching
{
    public class RedisCaching : IRedisCaching
    {
        
        private readonly IDistributedCache _distributedCache;
        private readonly RedisSetting _redisSetting;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCaching(IDistributedCache distributedCache, IConnectionMultiplexer redis)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = redis;
            _database = redis.GetDatabase();
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(TimeToLive)
            };

            var jsonOpt = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value, jsonOpt), cacheOpt);

        }

        // New method to invalidate cache by pattern
        public async Task InvalidateCacheByPattern(string pattern)
        {
           
            var server = GetRedisServer();
            var keys = server.Keys(pattern: pattern);
            if (keys == null || !keys.Any())
            {
                return;
            }
            foreach (var key in keys)
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        // Method to get Redis server from ConnectionMultiplexer
        private IServer GetRedisServer()
        {
            // Use ConnectionMultiplexer to get the Redis server
            var endpoint = _redisConnection.GetEndPoints().First(); // Get the first configured endpoint
            return _redisConnection.GetServer(endpoint);
        }

        /// <summary>
        /// Delete a key from Redis
        /// </summary>
        /// <param name="key">The key to be deleted</param>
        /// <returns>True if the key was removed, false if the key did not exist</returns>
        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Set redis with hash set data structure
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashEntries"></param>
        /// <param name="TimeToLive"></param>
        /// <returns></returns>
        public async Task HashSetAsync(string key, HashEntry[] hashEntries, int TimeToLive)
        {
            await _database.HashSetAsync(key, hashEntries);
            await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(TimeToLive));
        }

        /// <summary>
        /// Get a specific field value from a Redis hash
        /// </summary>
        /// <param name="key">The Redis key for the hash</param>
        /// <param name="field">The field name you want to retrieve</param>
        /// <returns>The value of the field as a string</returns>
        public async Task<string?> HashGetSpecificKeyAsync(string key, string field)
        {
            var value = await _database.HashGetAsync(key, field);
            return value.HasValue ? value.ToString() : null;
        }


        /// <summary>
        /// Get all the keys that related to keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<string> SearchKeysAsync(string keyword)
        {
            // Get the Redis server (works for a single server setup)
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

            // Search for keys that match the pattern (e.g., "user*")
            var keys = server.Keys(pattern: $"{keyword}*").Select(key => (string)key!).ToList();

            // Return the list of keys
            return keys;
        }

        /// <summary>
        /// Delete all keys that related to keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task FlushByRelatedKey(string keyword)
        {
            var keys = SearchKeysAsync(keyword);
            if (keys.Any())
            {
                await _database.KeyDeleteAsync(keys.Select(k => (RedisKey)k).ToArray());
            }
        }
    }
}
