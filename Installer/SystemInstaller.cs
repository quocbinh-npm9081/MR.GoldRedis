using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MR.GoldRedis.Installer
{
    public class SystemInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
        }
    }
}
