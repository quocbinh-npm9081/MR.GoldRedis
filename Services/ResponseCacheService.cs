using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace MR.GoldRedis.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _distributedCache = distributedCache;
        }
        public Task<string> GetCacheResonseAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;


            //Bình 
            var serializerResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Bước này là chuyển đổi Class => JSON (Với Key sẽ là kiểu camelCase(firstName) thay vì PascalCase(FirstName))
            });


            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            }); ;
        }
    }
}
