using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class VillageEntityTypeConfiguration : IEntityTypeConfiguration<Village>
{
    public void Configure(EntityTypeBuilder<Village> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Nom).IsRequired();
        builder.HasOne(v => v.SousPrefecture)
               .WithMany(sp => sp.Villages)
               .HasForeignKey(v => v.SousPrefectureId);
    }
}
