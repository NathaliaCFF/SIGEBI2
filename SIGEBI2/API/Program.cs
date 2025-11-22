using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Infraestructure.Dependencies;
using SIGEBI.Infrastructure.Dependencies;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURACIÓN DE SERVICIOS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CONFIGURACIÓN DE AUTENTICACIÓN JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// CONFIGURACIÓN DE SWAGGER + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIGEBI API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Registro de DbContext

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Registro de Repositorios

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IDetallePrestamoRepository, DetallePrestamoRepository>();
builder.Services.AddScoped<ILibroRepository, LibroRepository>();


// Registro de Servicios

builder.Services.AddScoped<ILibroService, LibroService>();


// Registro modular de dependencias adicionales

builder.Services.AddUsuarioDependencies();
builder.Services.AddPrestamoDependency();
builder.Services.AddDetallePrestamoDependency();
builder.Services.AddReporteDependency();
builder.Services.AddConfiguracionDependency();
builder.Services.AddAuthDependency();


// CONSTRUCCIÓN Y CONFIGURACIÓN DE LA APLICACIÓN

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
