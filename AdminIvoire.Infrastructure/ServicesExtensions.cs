using AdminIvoire.Infrastructure.ApiClient;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure;

/// <summary>
/// Extensions pour l'enregistrement des services de la couche Infrastructure
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Ajoute tous les services de la couche Infrastructure au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <param name="configuration">La configuration de l'application</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddGeocodingApiClient();
        services.AddOpenStreetMapApiClient();

        return services;
    }
}
