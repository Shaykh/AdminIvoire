using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.Persistence;

public static class DbSchemaUpdater
{
    public static async Task UpdateAsync<TContext>(this IServiceCollection services, CancellationToken cancellationToken) where TContext : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }
}
