using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence;

public class LocaliteContext(DbContextOptions<LocaliteContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocaliteContext).Assembly);
    }

    public DbSet<District> Districts { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Departement> Departements { get; set; }
    public DbSet<SousPrefecture> SousPrefectures { get; set; }
    public DbSet<Commune> Communes { get; set; }
    public DbSet<Village> Villages { get; set; }
}
