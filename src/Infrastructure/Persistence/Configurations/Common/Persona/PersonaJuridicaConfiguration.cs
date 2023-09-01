using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations.Common.Persona;

public class PersonaJuridicaConfiguration : IEntityTypeConfiguration<PersonasJuridicas>
{
    public void Configure(EntityTypeBuilder<PersonasJuridicas> builder)
    {
        builder.ToTable("PersonasJuridicas");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.RazonSocial);
        builder.Property(a => a.MarcaComercial);
        builder.Property(a => a.RepresentanteLegal);

        // Relationship with RepresentanteLegalDto
        builder.HasOne(a => a.RepresentanteLegalDto)
            .WithMany()
            .HasForeignKey(a => a.RepresentanteLegal);
    }
}