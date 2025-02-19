using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.Persistence;

public static class DbSchemaUpdater
{
    public static async Task ApplyMigrationAsync<TContext>(this IServiceCollection services, CancellationToken cancellationToken) where TContext : DbContext
    {
        var context = services.BuildServiceProvider().GetRequiredService<TContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }
}
