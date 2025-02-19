using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdminIvoire.Infrastructure.Persistence;

public class LocaliteContextDesignTimeFactory : IDesignTimeDbContextFactory<LocaliteContext>
{
    public LocaliteContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LocaliteContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AdminIvoire;Username=postgres;Password=postgres");
        return new LocaliteContext(optionsBuilder.Options);
    }
}
