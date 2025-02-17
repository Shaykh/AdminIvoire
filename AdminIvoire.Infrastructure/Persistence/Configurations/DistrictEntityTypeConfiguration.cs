using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class DistrictEntityTypeConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.HasKey(d => d.Id);
        builder.HasIndex(d => d.Nom).IsUnique();
        builder.Property(d => d.Nom).IsRequired();
        builder.OwnsOne(r => r.CoordonneesGeographiques);
    }
}
