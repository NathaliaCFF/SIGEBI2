using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Infraestructure.Dependencies
{
    public static class PrestamoDependency
    {
        public static IServiceCollection AddPrestamoDependency(this IServiceCollection services)
        {
            services.AddScoped<IPrestamoService, PrestamoService>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            return services;
        }
    }
}
