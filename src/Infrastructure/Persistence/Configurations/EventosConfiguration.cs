using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gestion_transportista_gestiones.Infrastructure.Persistence.Configurations;

public class EventosConfiguration : IEntityTypeConfiguration<Eventos>
{
  public void Configure(EntityTypeBuilder<Eventos> builder)
  {
    builder.ToTable("Eventos");
    builder.HasKey(a => a.ID_Evento);
    builder.Property(a => a.ID_Evento).ValueGeneratedOnAdd();
    builder.Property(e => e.Tipo_Evento);
    builder.Property(e => e.Evento);
    builder.Property(e => e.FechaHoraPublicado);
    builder.Property(e => e.FechaHoraAlta);
  }
}