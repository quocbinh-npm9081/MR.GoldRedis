using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MR.GoldRedis.Installer
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
