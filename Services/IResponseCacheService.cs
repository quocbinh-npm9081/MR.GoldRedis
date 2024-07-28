using System;
using System.Threading.Tasks;

namespace MR.GoldRedis.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCacheResonseAsync(string cacheKey);

        Task RemoveCacheResponse(string partent);

    }
}
