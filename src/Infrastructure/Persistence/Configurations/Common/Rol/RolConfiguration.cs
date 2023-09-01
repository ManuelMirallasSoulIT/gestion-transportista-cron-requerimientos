using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Requisito;

public class RolConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Codigo);
        builder.Property(r => r.Codigo);
        builder.Property(r => r.Nombre);
        builder.Property(r => r.Categoria);
        builder.Property(r => r.Nivel);
    }
}
