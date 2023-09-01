using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common.Entidades;

public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedores>
{
    public void Configure(EntityTypeBuilder<Proveedores> builder)
    {
        builder.ToTable("Proveedores");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Persona);
        builder.Property(a => a.FechaBloqueo);
        builder.Property(a => a.FechaAlta);
        builder.Property(a => a.Estado);

        // Relationship with Persona
        builder.HasOne(a => a.PersonaDto)
            .WithMany()
            .HasForeignKey(a => a.Persona);
    }
}
