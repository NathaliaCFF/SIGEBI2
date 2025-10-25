using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.Infrastructure.Dependencies
{
    public static class LibroDependency
    {
        public static void AddLibroDependencies(this IServiceCollection services)
        {
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ILibroService, LibroService>();
        }
    }
}
