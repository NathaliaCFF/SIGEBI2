using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Infraestructure.Dependencies
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddConfiguracionDependency(this IServiceCollection services)
        {
            services.AddScoped<IConfiguracionService, ConfiguracionService>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            return services;
        }
    }
}
