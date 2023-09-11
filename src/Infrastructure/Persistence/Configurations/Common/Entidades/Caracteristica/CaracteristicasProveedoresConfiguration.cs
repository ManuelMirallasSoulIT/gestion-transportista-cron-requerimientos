using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Persistence.Configurations.Common.Entidades.Caracteristica;

internal sealed class CaracteristicasProveedoresConfiguration : IEntityTypeConfiguration<CaracteristicasProveedores>
{
    public void Configure(EntityTypeBuilder<CaracteristicasProveedores> builder)
    {
        builder.ToTable("CaracteristicasProveedores").HasNoKey();

        builder.Property(a => a.Proveedor);
        builder.Property(a => a.Caracteristica);

        // Relationship with Caracteristica
        builder.HasOne(a => a.CaracteristicaDto)
            .WithMany()
            .HasForeignKey(a => a.Caracteristica);

        // Relationship with Proveedor
        builder.HasOne(a => a.ProveedorDto)
            .WithMany()
            .HasForeignKey(a => a.Proveedor);
    }
}
