using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class LibroConfiguration : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> builder)
        {
            builder.ToTable("Libros");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Titulo).IsRequired();
            builder.Property(l => l.Autor).IsRequired();
            builder.Property(l => l.ISBN).IsRequired();
            builder.Property(l => l.Editorial);
            builder.Property(l => l.Año).IsRequired();
            builder.Property(l => l.Categoria);
            builder.Property(l => l.Disponible).IsRequired();
        }
    }
}
