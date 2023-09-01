using Domain.Entities.Presentaciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace gestion_transportista_gestiones.Infrastructure.Persistence.Configurations
{
    public class RequisitosPresentadosConfiguration : IEntityTypeConfiguration<RequisitosPresentados>
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

        }
    }
}