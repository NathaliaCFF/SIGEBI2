using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.Infrastructure.Dependencies
{
    public static class LibroDependency
    {
        public static void AddLibroDependencies(this IServiceCollection services)
        {
            services.AddScoped<ILibroService, LibroService>();
        }
    }
}
