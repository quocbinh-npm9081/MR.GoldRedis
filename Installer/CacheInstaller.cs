using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MR.GoldRedis.Configurations;
using MR.GoldRedis.Services;
using StackExchange.Redis;

namespace MR.GoldRedis.Installer
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            //Binding json từ appsetting vào class RedisConfiguration
            var redisConfiguration = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);

            //Đăng ký redisConfiguration như một dịch vụ singleton, tức là một phiên bản duy nhất sẽ được tạo ra và sử dụng xuyên suốt vòng đời của ứng dụng.
            services.AddSingleton<RedisConfiguration>(redisConfiguration);

            //Kiểm tra xem Redis có được phép kích hoạt hay không:
            if (!redisConfiguration.Enabled)
                return;

            //IConnectionMultiplexer là một giao diện từ thư viện StackExchange.Redis để quản lý kết nối tới Redis.
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            services.AddStackExchangeRedisCache(opts => opts.Configuration = redisConfiguration.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }


    }
}