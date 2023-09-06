using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Persistence.Configurations.Common.Entidades.Caracteristica;

internal sealed class CaracteristicasChoferesConfiguration : IEntityTypeConfiguration<CaracteristicasChoferes>
{
    public void Configure(EntityTypeBuilder<CaracteristicasChoferes> builder)
    {
        builder.ToTable("CaracteristicasChoferes").HasNoKey();

        builder.Property(a => a.Chofer);
        builder.Property(a => a.Caracteristica);

        // Relationship with Caracteristica
        builder.HasOne(a => a.CaracteristicaDto)
            .WithMany()
            .HasForeignKey(a => a.Caracteristica);

        // Relationship with Chofer
        builder.HasOne(a => a.ChoferDto)
            .WithMany()
            .HasForeignKey(a => a.Chofer);
    }
}
