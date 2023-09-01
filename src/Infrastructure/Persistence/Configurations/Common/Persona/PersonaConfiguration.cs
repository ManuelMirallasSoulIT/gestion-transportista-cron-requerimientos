using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations.Common.Persona;

public class EstadosConfiguration : IEntityTypeConfiguration<Personas>
{
    public void Configure(EntityTypeBuilder<Personas> builder)
    {
        builder.ToTable("Personas");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Telefono);
        builder.Property(a => a.Email);
        builder.Property(a => a.Cuit);
        builder.Property(a => a.Cbu);
        builder.Property(a => a.RegimenEspecial);
        builder.Property(a => a.CondicionIva);
        builder.Property(a => a.TipoEmpresa);
        builder.Property(a => a.DomicilioFiscalCalle);
        builder.Property(a => a.DomicilioFiscalNumero);
        builder.Property(a => a.DomicilioFiscalPiso);
        builder.Property(a => a.DomicilioFiscalDepartamento);
        builder.Property(a => a.DomicilioFiscalCodigoPostal);
        builder.Property(a => a.DomicilioRealCalle);
        builder.Property(a => a.DomicilioRealNumero);
        builder.Property(a => a.DomicilioRealPiso);
        builder.Property(a => a.DomicilioRealDepartamento);
        builder.Property(a => a.DomicilioRealCodigoPostal);
        builder.Property(a => a.PersonaFisica);
        builder.Property(a => a.PersonaJuridica);

        // Relationship with PersonaFisica
        builder.HasOne(a => a.PersonaFisicaDto)
            .WithMany()
            .HasForeignKey(a => a.PersonaFisica);

        // Relationship with PersonaJuridica
        builder.HasOne(a => a.PersonaJuridicaDto)
            .WithMany()
            .HasForeignKey(a => a.PersonaJuridica);
    }
}