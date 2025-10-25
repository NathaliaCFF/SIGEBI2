using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.Infrastructure.Dependencies
{
    public static class UsuarioDependency
    {
        public static void AddUsuarioDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAuthService, AuthService>(); // ✅ agregado
            services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}
