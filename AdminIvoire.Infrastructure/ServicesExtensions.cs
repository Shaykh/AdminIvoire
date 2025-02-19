using AdminIvoire.Infrastructure.ApiClient;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddGeocodingApiClient();

        return services;
    }
}
