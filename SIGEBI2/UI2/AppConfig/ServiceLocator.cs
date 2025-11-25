using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
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
                return;

            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(_configuration);

            services.AddSingleton<SessionService>();
            services.AddSingleton<ValidationService>();
            services.AddSingleton<NotificationService>();

            services.AddHttpClient<ApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config.GetValue<string>("ApiSettings:BaseUrl");
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddTransient<AuthAdapter>();
            services.AddTransient<UsuarioAdapter>();
            services.AddTransient<LibroAdapter>();
            services.AddTransient<PrestamoAdapter>();
            services.AddTransient<ConfigurationAdapter>();

            _provider = services.BuildServiceProvider();
        }

        public static T GetRequired<T>() where T : notnull =>
            _provider!.GetRequiredService<T>();

        public static IConfiguration Configuration => _configuration!;

        public static SessionService SessionService => GetRequired<SessionService>();
        public static ValidationService ValidationService => GetRequired<ValidationService>();
        public static NotificationService NotificationService => GetRequired<NotificationService>();

        public static AuthAdapter AuthAdapter => GetRequired<AuthAdapter>();
        public static UsuarioAdapter UsuarioAdapter => GetRequired<UsuarioAdapter>();
        public static LibroAdapter LibroAdapter => GetRequired<LibroAdapter>();
        public static PrestamoAdapter PrestamoAdapter => GetRequired<PrestamoAdapter>();
        public static ConfigurationAdapter ConfigurationAdapter => GetRequired<ConfigurationAdapter>();
    }
}
