using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common.Persona;

public class PersonaFisicaConfiguration : IEntityTypeConfiguration<PersonasFisicas>
{
    public void Configure(EntityTypeBuilder<PersonasFisicas> builder)
    {
        builder.ToTable("PersonasFisicas");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Nombre);
        builder.Property(a => a.Apellido);
        builder.Property(a => a.Dni);
        builder.Property(a => a.Foto);
        builder.Property(a => a.FechaNacimiento);
        builder.Property(a => a.Nacionalidad);
        builder.Property(a => a.Genero);
        builder.Property(a => a.EstadoCivil);
    }
}
