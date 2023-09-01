using Domain.Entities.Presentacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Presentacion;

public class RequisitoPresentadoConfiguration : IEntityTypeConfiguration<RequisitosPresentados>
{
    public void Configure(EntityTypeBuilder<RequisitosPresentados> builder)
    {
        builder.ToTable("RequisitosPresentados");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Proveedor);
        builder.Property(a => a.Chofer);
        builder.Property(a => a.Unidad);
        builder.Property(a => a.Requisito);
        builder.Property(a => a.Vencimiento);
        builder.Property(a => a.Documento);
        builder.Property(a => a.Status);
        builder.Property(a => a.FechaPresentacion);
        builder.Property(a => a.FechaAprobacion);
        builder.Property(a => a.UsuarioAprobacion);
        builder.Property(a => a.MotivoRechazo);

        // Relationship with Proveedor
        builder.HasOne(a => a.ProveedorDto)
            .WithMany()
            .HasForeignKey(a => a.Proveedor);

        // Relationship with Unidad
        builder.HasOne(a => a.UnidadDto)
            .WithMany()
            .HasForeignKey(a => a.Unidad);

        // Relationship with Requisito
        builder.HasOne(a => a.RequisitoDto)
            .WithMany()
            .HasForeignKey(a => a.Requisito);

        // Relationship with Chofer
        builder.HasOne(a => a.ChoferDto)
            .WithMany()
            .HasForeignKey(a => a.Chofer);

    }
}

