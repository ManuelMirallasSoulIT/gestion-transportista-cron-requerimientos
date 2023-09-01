using GDT.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Requisito;

public class RequisitoRolConfiguration : IEntityTypeConfiguration<RequisitoRol>
{
    public void Configure(EntityTypeBuilder<RequisitoRol> builder)
    {
        builder.ToTable("RequisitoRol");
        builder.HasKey(rr => rr.Id);
        builder.Property(rr => rr.Id).ValueGeneratedOnAdd();
        builder.Property(rr => rr.Requisito).HasColumnName("Requisito"); 
        builder.Property(rr => rr.Rol).HasColumnName("Rol");
        builder.Property(rr => rr.Tipo).HasColumnName("Tipo");

        builder.HasOne(rr => rr.RequisitoDto)
            .WithMany(r => r.RequisitoRolDto)
            .HasForeignKey(rr => rr.Requisito)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(rr => rr.RolDto)
            .WithMany() 
            .HasForeignKey(rr => rr.Rol);
    }
}
