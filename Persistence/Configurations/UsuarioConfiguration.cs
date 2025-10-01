using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nombre)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.Contraseña)
                   .IsRequired();

            builder.Property(u => u.Rol)
                   .HasMaxLength(50)
                   .HasDefaultValue("Usuario");

            builder.Property(u => u.Activo)
                   .IsRequired();
        }
    }
}
