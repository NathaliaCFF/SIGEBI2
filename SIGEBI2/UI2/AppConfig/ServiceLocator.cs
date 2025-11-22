using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Buses.Configuration;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System.Net.Http.Headers;
using UI2.Adapters;
using UI2.Services;

namespace UI2.AppConfig
{
    internal static class ServiceLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;

        public static IConfiguracionService ConfiguracionService =>
            GetRequired<IConfiguracionService>();

        // CONFIGURAR CONTENEDOR

        public static void Configure()
        {
            if (_provider != null)
                return;

            var services = new ServiceCollection();

            // ----------- Cargar configuración -------------------------
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            services.AddSingleton<IConfiguration>(_configuration);

            // ----------- Servicios UI --------------------------------
            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();

            // ----------- HttpClient para módulos API ------------------
            services.AddHttpClient<ApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config.GetValue<string>("ApiSettings:BaseUrl");

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("Debe configurar ApiSettings:BaseUrl.");

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // ----------- Adaptadores API -------------------------------
            services.AddTransient<AuthAdapter>();
            services.AddTransient<UsuarioAdapter>();
            services.AddTransient<LibroAdapter>();
            services.AddTransient<PrestamoAdapter>();

            // ----------- DbContext (solo para módulo Configuración) ---
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection")));

            // ----------- Repositorios (Scoped) -------------------------
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

            // ----------- Servicios Application Layer ------------------
            services.AddScoped<IConfiguracionService, ConfiguracionService>();

            // ----------- BUS Handlers ---------------------------------
            services.AddTransient<GetConfiguracionHandler>();
            services.AddTransient<UpdateConfiguracionHandler>();

            // ----------- Construir el contenedor -----------------------
            _provider = services.BuildServiceProvider();
        }


        // MÉTODO AUXILIAR

        public static T GetRequired<T>() where T : notnull
        {
            if (_provider == null)
                throw new InvalidOperationException(
                    "El ServiceLocator no fue configurado. Llama a Configure().");

            return _provider.GetRequiredService<T>();
        }

        public static IConfiguration Configuration =>
            _configuration ?? throw new InvalidOperationException("La configuración no está disponible.");

        // ------------ UI Services -----------------------------------
        public static SessionService SessionService => GetRequired<SessionService>();
        public static ValidationService ValidationService => GetRequired<ValidationService>();
        public static NotificationService NotificationService => GetRequired<NotificationService>();

        // ------------ API Adapters ----------------------------------
        public static AuthAdapter AuthAdapter => GetRequired<AuthAdapter>();
        public static UsuarioAdapter UsuarioAdapter => GetRequired<UsuarioAdapter>();
        public static LibroAdapter LibroAdapter => GetRequired<LibroAdapter>();
        public static PrestamoAdapter PrestamoAdapter => GetRequired<PrestamoAdapter>();

        // ------------ BUS Handlers ----------------------------------
        public static GetConfiguracionHandler GetConfiguracionHandler =>
            GetRequired<GetConfiguracionHandler>();

        public static UpdateConfiguracionHandler UpdateConfiguracionHandler =>
            GetRequired<UpdateConfiguracionHandler>();
    }
}
