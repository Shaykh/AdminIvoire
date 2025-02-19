using AdminIvoire.Application.Parametrage;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence;

public class LocaliteContext(DbContextOptions<LocaliteContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommuneEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DepartementEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DistrictEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RegionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SousPrefectureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VillageEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new ParametrageEntityTypeConfiguration());
    }

    public DbSet<District> Districts { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Departement> Departements { get; set; }
    public DbSet<SousPrefecture> SousPrefectures { get; set; }
    public DbSet<Commune> Communes { get; set; }
    public DbSet<Village> Villages { get; set; }

    public DbSet<ParametrageEntity> Parametrages { get; set; }
}
