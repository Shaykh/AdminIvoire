using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class DepartementEntityTypeConfiguration : IEntityTypeConfiguration<Departement>
{
    public void Configure(EntityTypeBuilder<Departement> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Nom).IsRequired();
        builder.HasOne(d => d.Region)
               .WithMany(r => r.Departements)
               .HasForeignKey(d => d.RegionId);
    }
}
