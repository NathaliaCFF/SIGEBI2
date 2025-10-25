using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        // Constructor usado por inyección de dependencias
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Constructor sin parámetros
        public AppDbContext() { }

        // Tablas (DbSet)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<DetallePrestamo> DetallePrestamos { get; set; }
        public DbSet<Configuration> Configuraciones { get; set; }

        // Configuración global del modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        // Configuración por defecto 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

                if (processName.Contains("testhost", StringComparison.OrdinalIgnoreCase) ||
                    processName.Contains("vstest", StringComparison.OrdinalIgnoreCase))
                {
                    // Base exclusiva para pruebas unitarias
                    optionsBuilder.UseSqlServer(
                        "Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;");
                }
                else
                {
                    // Base de datos oficial del sistema
                    optionsBuilder.UseSqlServer(
                        "Server=.;Database=SIGEBI;Trusted_Connection=True;TrustServerCertificate=True;");
                }
            }
        }

    }
}

