using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Infrastructure.Dependencies;
using SIGEBI.Infraestructure.Dependencies;
using SIGEBI.Persistence.Dependencies;
using UI2.Adapters;
using UI2.Services;

namespace UI2.AppConfig
{
    internal static class ServiceLocator
    {
        private static readonly object _sync = new();
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;

        public static void Configure()
        {
            if (_provider != null)
            {
                return;
            }

            lock (_sync)
            {
                if (_provider != null)
                {
                    return;
                }

                var services = new ServiceCollection();

                _configuration = BuildConfiguration();
                services.AddSingleton(_configuration);

                services.AddPersistence(_configuration);
                RegisterApplicationServices(services);
                RegisterUiServices(services);
                RegisterAdapters(services);

                _provider = services.BuildServiceProvider();
            }
        }

        private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            UsuarioDependency.AddUsuarioDependencies(services);
            LibroDependency.AddLibroDependencies(services);
            PrestamoDependency.AddPrestamoDependency(services);
            DetallePrestamoDependency.AddDetallePrestamoDependency(services);
            ReporteDependency.AddReporteDependency(services);
            ConfiguracionDependency.AddConfiguracionDependency(services);
            AuthDependency.AddAuthDependency(services);
        }

        private static void RegisterUiServices(IServiceCollection services)
        {
            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();
        }

        private static void RegisterAdapters(IServiceCollection services)
        {
            services.AddTransient<AuthAdapter>();
            services.AddTransient<UsuarioAdapter>();
            services.AddTransient<LibroAdapter>();
            services.AddTransient<PrestamoAdapter>();
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