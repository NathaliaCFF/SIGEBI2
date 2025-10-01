using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class DetallePrestamoConfiguration : IEntityTypeConfiguration<DetallePrestamo>
    {
        public void Configure(EntityTypeBuilder<DetallePrestamo> builder)
        {
            builder.ToTable("DetallePrestamos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Devuelto).IsRequired();
            builder.Property(d => d.FechaDevolucion);

            builder.HasOne(d => d.Prestamo)
                   .WithMany(p => p.Detalles)
                   .HasForeignKey(d => d.PrestamoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Libro)
                   .WithMany()
                   .HasForeignKey(d => d.LibroId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
