namespace Infrastructure.Caching
{
    public interface IRedisCaching
    {
        // Get cache value by key
        Task<T> GetAsync<T>(string key);
        // Set cache value by key
        Task SetAsync<T>(string key, T value);
        // Remove cache value by key
        Task RemoveAsync(string key);
    }
}
