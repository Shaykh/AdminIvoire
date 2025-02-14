using AdminIvoire.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();

        return services;
    }
}
