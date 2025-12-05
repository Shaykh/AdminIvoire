using AdminIvoire.Application.ApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.ApiClient;

/// <summary>
/// Extensions pour l'enregistrement des clients API
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Ajoute le client API de géocodage (Google Geocoding) au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddGeocodingApiClient(this IServiceCollection services)
    {
        services.AddHttpClient<IGeocodingApiClient, GoogleGeocodingApiClient>();

        return services;
    }

    /// <summary>
    /// Ajoute le client API OpenStreetMap au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddOpenStreetMapApiClient(this IServiceCollection services)
    {
        services.AddHttpClient<IOpenStreetMapApiClient, OpenStreetMapApiClient>();

        return services;
    }
}
