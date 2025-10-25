using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Infraestructure.Dependencies
{
    public static class DetallePrestamoDependency
    {
        public static IServiceCollection AddDetallePrestamoDependency(this IServiceCollection services)
        {
            services.AddScoped<IDetallePrestamoRepository, DetallePrestamoRepository>();
            return services;
        }
    }
}
