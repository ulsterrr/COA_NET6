﻿namespace Application.Interfaces.Services
{
    public interface ICacheService
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key, CancellationToken token = default);

        void Refresh(string key);
        Task RefreshAsync(string key, CancellationToken token = default);

        void Remove(string key);
        Task RemoveAsync(string key, CancellationToken token = default);

        void Set<T>(string key, T value, TimeSpan? slidingExpiration = null);
        Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

        void ClearAll();
        Task ClearAllAsync();

        void RemoveByPrefix(string prefix);
        Task RemoveByPrefixAsync(string prefix);
    }
}
