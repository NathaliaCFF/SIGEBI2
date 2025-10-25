using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Infraestructure.Dependencies
{
    public static class ReporteDependency
    {
        public static IServiceCollection AddReporteDependency(this IServiceCollection services)
        {
            services.AddScoped<IReporteService, ReporteService>();
            services.AddScoped<IReporteRepository, ReporteRepository>();
            return services;
        }
    }
}

