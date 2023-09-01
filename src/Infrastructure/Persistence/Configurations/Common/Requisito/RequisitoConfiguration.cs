using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Requisito;

public class RequisitoConfiguration : IEntityTypeConfiguration<Requisitos>
{
    public void Configure(EntityTypeBuilder<Requisitos> builder)
    {
        builder.ToTable("Requisitos");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.NombreAbreviado);
        builder.Property(r => r.Nombre);
        builder.Property(r => r.Caracteristica);
        builder.Property(r => r.Entidad);
        builder.Property(r => r.Presentacion);
        builder.Property(r => r.TipoVencimiento);
        builder.Property(r => r.Tolerancia);
        builder.Property(r => r.Observaciones);
        builder.Property(r => r.Activo);
        builder.Property(r => r.PermiteFoto);
        builder.Property(r => r.Archivo);
        builder.Property(r => r.UserChange);
        builder.Property(r => r.Clasificacion);

        // Relationship with Caracteristica
        builder.HasOne(a => a.CaracteristicaDto)
            .WithMany()
            .HasForeignKey(a => a.Caracteristica);

        builder.HasMany(r => r.RequisitoRolDto)
            .WithOne()
            .HasForeignKey(rr => rr.Requisito);

        builder.HasMany(r => r.EstadoRequisitoDto)
           .WithOne()
           .HasForeignKey(er => er.Requisito);
    }
}
