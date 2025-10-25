using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Repository;
using SIGEBI.Infraestructure.Dependencies;
using SIGEBI.Infrastructure.Dependencies;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// CONFIGURACIÓN DE SERVICIOS
// =====================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ?? Registro de DbContext con cadena de conexión desde appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ?? Registro modular de dependencias (repositorios y servicios)
builder.Services.AddUsuarioDependencies();
builder.Services.AddLibroDependencies();
builder.Services.AddPrestamoDependency();
builder.Services.AddDetallePrestamoDependency();
builder.Services.AddReporteDependency();
builder.Services.AddConfiguracionDependency();
builder.Services.AddAuthDependency();


// =====================================================
// CONSTRUCCIÓN Y CONFIGURACIÓN DE LA APLICACIÓN
// =====================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
