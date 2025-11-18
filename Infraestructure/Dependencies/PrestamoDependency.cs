using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.Infraestructure.Dependencies
{
    public static class PrestamoDependency
    {
        public static IServiceCollection AddPrestamoDependency(this IServiceCollection services)
        {
            services.AddScoped<IPrestamoService, PrestamoService>();
            return services;
        }
    }
}
