using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Configurations
{
    public class ReporteConfiguration : IEntityTypeConfiguration<Reporte>
    {
        public void Configure(EntityTypeBuilder<Reporte> builder)
        {
            // Esta entidad no se mapea a tabla porque es de solo consulta.
            builder.HasNoKey();
        }
    }
}

