using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using UI2.Services;
using UI2.Services.Implementations;
using UI2.Services.Interfaces;

namespace UI2.AppConfig
{
    internal static class ServiceLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;

        public static void Configure()
        {
            if (_provider != null)
                return;

            var services = new ServiceCollection();


            // Configuración general (appsettings.json)

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(_configuration);


            // Servicios de utilidad

            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();


            // Servicios API 

            services.AddTransient<IAuthApiService, AuthApiService>();
            services.AddTransient<IUsuarioApiService, UsuarioApiService>();
            services.AddTransient<ILibroApiService, LibroApiService>();
            services.AddTransient<IPrestamoApiService, PrestamoApiService>();
            services.AddTransient<IConfigurationApiService, ConfigurationApiService>();

            // HttpClient utilizado por ApiClient

            services.AddHttpClient<ApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var baseUrl = config.GetValue<string>("ApiSettings:BaseUrl")
                             ?? throw new InvalidOperationException("ApiSettings:BaseUrl no está configurado.");

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Construcción final del contenedor de dependencias
            _provider = services.BuildServiceProvider();
        }


        // Métodos para obtener servicios registrados

        public static T GetRequired<T>() where T : notnull =>
            _provider!.GetRequiredService<T>();

        public static IConfiguration Configuration => _configuration!;

        public static SessionService SessionService => GetRequired<SessionService>();
        public static ValidationService ValidationService => GetRequired<ValidationService>();
        public static NotificationService NotificationService => GetRequired<NotificationService>();


        public static IAuthApiService AuthApiService => GetRequired<IAuthApiService>();
        public static IUsuarioApiService UsuarioApiService => GetRequired<IUsuarioApiService>();
        public static ILibroApiService LibroApiService => GetRequired<ILibroApiService>();
        public static IPrestamoApiService PrestamoApiService => GetRequired<IPrestamoApiService>();
        public static IConfigurationApiService ConfigurationApiService => GetRequired<IConfigurationApiService>();
    }
}
