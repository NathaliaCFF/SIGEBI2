using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;


namespace SIGEBI.Infraestructure.Dependencies
{
    public static class ConfiguracionDependency
    {
        public static IServiceCollection AddConfiguracionDependency(this IServiceCollection services)
        {
            services.AddScoped<IConfiguracionService, ConfiguracionService>();
            return services;
        }
    }
}
