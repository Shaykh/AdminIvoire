namespace AdminIvoire.WebApi.BackgroundServices;

/// <summary>
/// Extensions pour l'enregistrement des services en arrière-plan
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Ajoute tous les services en arrière-plan (hosted services) au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<InitialisationDonneesLocalitePopulationBackgroundService>();
        services.AddHostedService<RecuperationDonneesGeographiqueBackgroundService>();
        services.AddHostedService<AjoutVillagesAutomatiqueBackgroundService>();

        return services;
    }
}
