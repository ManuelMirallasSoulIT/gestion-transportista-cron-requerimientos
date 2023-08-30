using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gestion_transportista_gestiones.Infrastructure.Persistence.Configurations
{
    public class RequerimientosConfiguration : IEntityTypeConfiguration<Requerimientos>
    {
        public void Configure(EntityTypeBuilder<Requerimientos> builder)
        {
            builder.ToTable("Requerimientos");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Requisito);
            builder.Property(r => r.Chofer);
            builder.Property(r => r.Proveedor);
            builder.Property(r => r.Unidad);
            builder.Property(r => r.Estado);
            builder.Property(r => r.Presentacion);
            builder.Property(r => r.FechaCreacion);
            builder.Property(r => r.FechaModificacion);
            builder.Property(r => r.UsuarioCreacion);
            builder.Property(r => r.UsuarioModificacion);               
        }
    }
}
