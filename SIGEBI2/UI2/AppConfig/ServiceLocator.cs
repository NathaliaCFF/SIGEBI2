using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UI2.Adapters;
using UI2.Services;
using System.Net.Http;

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
                return;

            lock (_sync)
            {
                if (_provider != null)
                    return;

                var services = new ServiceCollection();

                // Configuración
                _configuration = BuildConfiguration();
                services.AddSingleton(_configuration);

                // ======================================================
                // REGISTRO CORRECTO DEL HttpClient
                // ======================================================
                services.AddHttpClient("ApiClient", client =>
                {
                    string? baseUrl = _configuration["Api:BaseUrl"];

                    if (string.IsNullOrWhiteSpace(baseUrl))
                        throw new Exception("Api:BaseUrl no está configurado.");

                    client.BaseAddress = new Uri(baseUrl);
                });

                // ======================================================
                // Servicios propios de UI
                // ======================================================
                services.AddSingleton<SessionService>();
                services.AddSingleton<ValidationService>();
                services.AddSingleton<NotificationService>();

                // ======================================================
                // Adapters que consumen la API
                // ======================================================
                services.AddTransient<AuthAdapter>(sp =>
                {
                    var client = sp.GetRequiredService<IHttpClientFactory>()
                                   .CreateClient("ApiClient");
                    return new AuthAdapter(client);
                });

                services.AddTransient<UsuarioAdapter>(sp =>
                {
                    var client = sp.GetRequiredService<IHttpClientFactory>()
                                   .CreateClient("ApiClient");
                    return new UsuarioAdapter(client);
                });

                services.AddTransient<LibroAdapter>(sp =>
                {
                    var client = sp.GetRequiredService<IHttpClientFactory>()
                                   .CreateClient("ApiClient");
                    return new LibroAdapter(client);
                });

                services.AddTransient<PrestamoAdapter>(sp =>
                {
                    var client = sp.GetRequiredService<IHttpClientFactory>()
                                   .CreateClient("ApiClient");
                    return new PrestamoAdapter(client);
                });

                // Crear el ServiceProvider final
                _provider = services.BuildServiceProvider();
            }
        }

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

        // Métodos de acceso
        public static T GetRequired<T>() where T : notnull =>
            _provider!.GetRequiredService<T>();

        public static SessionService SessionService => GetRequired<SessionService>();
        public static ValidationService ValidationService => GetRequired<ValidationService>();
        public static NotificationService NotificationService => GetRequired<NotificationService>();

        public static AuthAdapter AuthAdapter => GetRequired<AuthAdapter>();
        public static UsuarioAdapter UsuarioAdapter => GetRequired<UsuarioAdapter>();
        public static LibroAdapter LibroAdapter => GetRequired<LibroAdapter>();
        public static PrestamoAdapter PrestamoAdapter => GetRequired<PrestamoAdapter>();
    }
}

