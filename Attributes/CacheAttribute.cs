using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MR.GoldRedis.Configurations;
using MR.GoldRedis.Services;

namespace MR.GoldRedis.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        //private readonly RedisConfiguration _redisConfiguration;

        public CacheAttribute(int timeToLiveSeconds = 1000)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
            //_redisConfiguration = redisConfiguration;
        }

        private static string GeneraCacheKeyFormRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }

        //OnActionExecutionAsync là hàm thuộc IAsyncActionFilter được khỏi động trước khi Action của Controller được gọi và sau khi Action ủa Controller kết thúc
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            #region Thực khi trước khi vào Action của controller
            //Check cache if exists or not ?
            //Lấy các config đã được đăng kí trong Container (Có thể sử dụng DI thay cho cách này)
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!cacheConfiguration.Enabled)
            {
                await next();
                return;
            }
            //Lấy cache thông qua context của Container (có thể sử dụng DI)
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GeneraCacheKeyFormRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCacheResonseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    ContentType = "application/json",
                    Content = cacheResponse,
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }


            #endregion

            #region thực thi sau khi action controller đã chạy
            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult objectResult)
            {
                await cacheService.SetCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
            #endregion
        }
    }
}
