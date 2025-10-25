using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class LibroConfiguration : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> b)
        {
            b.ToTable("Libros");
            b.HasKey(l => l.Id);

            
            b.Property(l => l.Titulo).IsRequired().HasMaxLength(150);
            b.Property(l => l.Autor).IsRequired().HasMaxLength(120);
            b.Property(l => l.ISBN).IsRequired().HasMaxLength(20);
            b.HasIndex(l => l.ISBN).IsUnique(); // Evita duplicados de ISBN

            
            b.Property(l => l.Editorial).HasMaxLength(80);
            b.Property(l => l.Categoria).HasMaxLength(80);
            b.Property(l => l.AnioPublicacion)
                .HasColumnName("AnioPublicacion")
                .IsRequired();

            
            b.Property(l => l.Disponible)
                .IsRequired()
                .HasDefaultValue(true);

            
            b.Property(l => l.Activo).HasDefaultValue(true);
            b.Property(l => l.FechaCreacion).HasDefaultValueSql("SYSDATETIME()");
            b.Property(l => l.FechaModificacion).IsRequired(false);

            
            b.HasIndex(l => l.Titulo);
            b.HasIndex(l => l.Autor);
        }
    }
}
