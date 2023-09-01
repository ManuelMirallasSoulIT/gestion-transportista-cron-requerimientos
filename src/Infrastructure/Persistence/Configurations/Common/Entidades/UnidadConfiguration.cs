using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common.Entidades;

public class UnidadConfiguration : IEntityTypeConfiguration<Unidades>
{
    public void Configure(EntityTypeBuilder<Unidades> builder)
    {
        builder.ToTable("Unidades");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Proveedor);
        builder.Property(a => a.TipoUnidad);
        builder.Property(a => a.Modelo);
        builder.Property(a => a.Titularidad);
        builder.Property(a => a.Uso);
        builder.Property(a => a.CategoriaTransporteCarga);
        builder.Property(a => a.TipoCombustible);
        builder.Property(a => a.TecnologiaMotor);
        builder.Property(a => a.TipoCombustibleOtro);
        builder.Property(a => a.PalaTipo);
        builder.Property(a => a.SeparadorCabinaMaterial);
        builder.Property(a => a.CajaTipo);
        builder.Property(a => a.DomicilioRealCodigoPostal);
        builder.Property(a => a.Estado);

        // Relationship with ProvedorDto
        builder.HasOne(a => a.ProvedorDto)
            .WithMany()
            .HasForeignKey(a => a.Proveedor);
    }
}
