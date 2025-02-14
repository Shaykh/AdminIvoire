using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class SousPrefectureEntityTypeConfiguration : IEntityTypeConfiguration<SousPrefecture>
{
    public void Configure(EntityTypeBuilder<SousPrefecture> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Nom).IsRequired();
        builder.HasOne(sp => sp.Departement)
               .WithMany(d => d.SousPrefectures)
               .HasForeignKey(sp => sp.DepartementId);
    }
}
