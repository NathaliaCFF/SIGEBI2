using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.ToTable("Configuraciones");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.DuracionPrestamoDias)
                   .IsRequired();
        }
    }
}
