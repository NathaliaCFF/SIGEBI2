using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<DetallePrestamo> DetallePrestamos { get; set; }
        public DbSet<Configuration> Configuraciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica automáticamente todas las configuraciones desde la carpeta Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

