using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class RegionEntityTypeConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(d => d.Nom).IsUnique();
        builder.Property(r => r.Nom).IsRequired();
        builder.HasOne(r => r.District)
               .WithMany(d => d.Regions)
               .HasForeignKey(r => r.DistrictId);
    }
}
