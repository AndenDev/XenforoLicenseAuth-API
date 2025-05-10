using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<IEd25519SigningService, Ed25519SigningService>();

            services.AddMemoryCache();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddDbContext<XenforoDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("XenForoDb"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("XenForoDb"))
                )
            );

            services.AddScoped<IXenforoAuthService, XenForoAuthService>();
            return services;
        }
    }
}
