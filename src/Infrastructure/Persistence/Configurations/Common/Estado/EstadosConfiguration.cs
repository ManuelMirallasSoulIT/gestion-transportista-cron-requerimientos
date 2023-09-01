using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common.Estado;

public class EstadosConfiguration : IEntityTypeConfiguration<Estados>
{
    public void Configure(EntityTypeBuilder<Estados> builder)
    {
        builder.ToTable("Estados");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Nombre);
        builder.Property(a => a.Tipo);
        builder.Property(a => a.Descripcion);
    }
}