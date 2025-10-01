using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
    {
        public void Configure(EntityTypeBuilder<Prestamo> builder)
        {
            builder.ToTable("Prestamos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FechaPrestamo).IsRequired();
            builder.Property(p => p.FechaVencimiento).IsRequired();
            builder.Property(p => p.Activo).IsRequired();

            // Relación con usuario
            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Prestamos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación con detalles
            builder.HasMany(p => p.Detalles)
                   .WithOne(d => d.Prestamo)
                   .HasForeignKey(d => d.PrestamoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
