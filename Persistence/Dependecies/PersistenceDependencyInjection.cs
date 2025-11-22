using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Persistence.Dependencies
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connection = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connection))
                {
                    throw new InvalidOperationException(
                        "No se encontró la cadena de conexión 'DefaultConnection'.");
                }

                options.UseSqlServer(connection);
            });

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddScoped<IDetallePrestamoRepository, DetallePrestamoRepository>();
            services.AddScoped<IReporteRepository, ReporteRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

            return services;
        }
    }
}