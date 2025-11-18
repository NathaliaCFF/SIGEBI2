using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.Infrastructure.Dependencies
{
    public static class UsuarioDependency
    {
        public static void AddUsuarioDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>(); 
            services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}
