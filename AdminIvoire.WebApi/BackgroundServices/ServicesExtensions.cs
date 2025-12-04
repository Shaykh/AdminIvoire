namespace AdminIvoire.WebApi.BackgroundServices;

public static class ServicesExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<InitialisationDonneesLocalitePopulationBackgroundService>();
        services.AddHostedService<RecuperationDonneesGeographiqueBackgroundService>();
        services.AddHostedService<AjoutVillagesAutomatiqueBackgroundService>();

        return services;
    }
}
