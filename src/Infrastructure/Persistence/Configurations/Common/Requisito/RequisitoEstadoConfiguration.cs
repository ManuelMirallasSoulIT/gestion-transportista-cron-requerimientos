using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Requisito;

public class RequisitoEstadoConfiguration : IEntityTypeConfiguration<EstadoRequisito>
{
    public void Configure(EntityTypeBuilder<EstadoRequisito> builder)
    {
        builder.ToTable("EstadoRequisito");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Estado);
        builder.Property(a => a.Requisito);

        builder.HasOne(a => a.RequisitoDto)
            .WithMany()
            .HasForeignKey(a => a.Requisito);
    }
}
