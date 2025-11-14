using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Infrastructure.Dependencies;
using SIGEBI.Infraestructure.Dependencies;
using SIGEBI.Persistence.Context;
using UI2.Adapters;
using UI2.Services;

namespace UI2.AppConfig
{
    internal static class ServiceLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;

        public static void Configure()
        {
            if (_provider != null)
            {
                return;
            }

            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            services.AddSingleton<IConfiguration>(_configuration);

            services.AddDbContext<AppDbContext>(options =>
            {
                var connection = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connection))
                {
                    throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");
                }

                options.UseSqlServer(connection);
            });

            services.AddUsuarioDependencies();
            services.AddLibroDependencies();
            services.AddPrestamoDependency();
            services.AddDetallePrestamoDependency();
            services.AddReporteDependency();
            services.AddConfiguracionDependency();
            services.AddAuthDependency();

            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();

            services.AddTransient<AuthAdapter>();
            services.AddTransient<UsuarioAdapter>();
            services.AddTransient<LibroAdapter>();
            services.AddTransient<PrestamoAdapter>();

            _provider = services.BuildServiceProvider();
        }

        public static T GetRequired<T>() where T : notnull
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("El ServiceLocator no fue configurado. Llama a Configure() al iniciar la aplicación.");
            }

            return _provider.GetRequiredService<T>();
        }

        public static IConfiguration Configuration => _configuration ?? throw new InvalidOperationException("La configuración no está disponible.");

        public static SessionService SessionService => GetRequired<SessionService>();
        public static ValidationService ValidationService => GetRequired<ValidationService>();
        public static NotificationService NotificationService => GetRequired<NotificationService>();
        public static AuthAdapter AuthAdapter => GetRequired<AuthAdapter>();
        public static UsuarioAdapter UsuarioAdapter => GetRequired<UsuarioAdapter>();
        public static LibroAdapter LibroAdapter => GetRequired<LibroAdapter>();
        public static PrestamoAdapter PrestamoAdapter => GetRequired<PrestamoAdapter>();
    }
}