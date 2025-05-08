using Application.Interfaces;
using Application.Interfaces.Xenforo;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Xenforo;
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
            services.AddSingleton<IMySqlDatabaseConnection, MySqlDatabaseConnection>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IXenforoAuthService, XenForoAuthService>();


            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IClientBuildRepository, ClientBuildRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IHwidBanListRepository, HwidBanListRepository>();
            services.AddScoped<IHwidResetRequestRepository, HwidResetRequestRepository>();
            services.AddScoped<ILicenseActivationLogRepository, LicenseActivationLogRepository>();
            services.AddScoped<ILicensePlanRepository, LicensePlanRepository>();
            services.AddScoped<ILicenseSessionLogRepository, LicenseSessionLogRepository>();
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

            return services;
        }
    }
}
