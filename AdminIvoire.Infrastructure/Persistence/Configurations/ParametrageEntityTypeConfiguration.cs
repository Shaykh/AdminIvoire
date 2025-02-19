using AdminIvoire.Application.Parametrage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class ParametrageEntityTypeConfiguration : IEntityTypeConfiguration<ParametrageEntity>
{
    public void Configure(EntityTypeBuilder<ParametrageEntity> builder)
    {
        builder.ToTable("Parametrage");
        builder.HasKey(e => e.Key);
        builder.Property(e => e.Key).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Value).HasMaxLength(50).IsRequired();
    }
}
