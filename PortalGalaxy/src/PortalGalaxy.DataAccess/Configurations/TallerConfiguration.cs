using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalGalaxy.Entities;

namespace PortalGalaxy.DataAccess.Configurations;

public class TallerConfiguration : IEntityTypeConfiguration<Taller>
{
    public void Configure(EntityTypeBuilder<Taller> builder)
    {
        builder.ToTable(nameof(Taller));

        builder.Property(p => p.PortadaUrl) 
            .IsUnicode(false) //VARCHAR
            .HasMaxLength(500);
        
        builder.Property(p => p.TemarioUrl)
            .IsUnicode(false)
            .HasMaxLength(500);

        builder.HasQueryFilter(p => p.Estado);
    }
}