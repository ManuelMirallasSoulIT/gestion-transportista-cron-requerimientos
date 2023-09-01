using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common.Entidades;

public class ChoferConfiguration : IEntityTypeConfiguration<Choferes>
{
    public void Configure(EntityTypeBuilder<Choferes> builder)
    {
        builder.ToTable("Choferes");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Proveedor);
        builder.Property(a => a.Persona);
        builder.Property(a => a.FechaBloqueo);
        builder.Property(a => a.FechaAlta);
        builder.Property(a => a.Activo);
        builder.Property(a => a.Estado);

        // Relationship with Proveedor
        builder.HasOne(a => a.ProvedorDto)
            .WithMany()
            .HasForeignKey(a => a.Proveedor);

        // Relationship with Persona
        builder.HasOne(a => a.PersonaDto)
            .WithMany()
            .HasForeignKey(a => a.Persona);
    }
}
