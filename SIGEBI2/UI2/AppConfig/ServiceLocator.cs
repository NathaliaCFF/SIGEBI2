using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using System.Windows.Forms;
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

            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();

            services.AddHttpClient<ApiClient>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new InvalidOperationException("Debe configurar ApiSettings:BaseUrl en appsettings.json.");
                }

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

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

