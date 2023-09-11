using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Persistence.Configurations.Common.Entidades.Caracteristica;

internal sealed class CaracteristicasUnidadesConfiguration : IEntityTypeConfiguration<CaracteristicasUnidades>
{
    public void Configure(EntityTypeBuilder<CaracteristicasUnidades> builder)
    {
        builder.ToTable("CaracteristicasUnidades").HasNoKey();

        builder.Property(a => a.Unidad);
        builder.Property(a => a.Caracteristica);

        // Relationship with Caracteristica
        builder.HasOne(a => a.CaracteristicaDto)
            .WithMany()
            .HasForeignKey(a => a.Caracteristica);

        // Relationship with Unidad
        builder.HasOne(a => a.UnidadDto)
            .WithMany()
            .HasForeignKey(a => a.Unidad);
    }
}
