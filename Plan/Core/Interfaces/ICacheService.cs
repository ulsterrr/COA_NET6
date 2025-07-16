using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, int expirationInSeconds);
        Task RemoveAsync(string key);
    }
}
