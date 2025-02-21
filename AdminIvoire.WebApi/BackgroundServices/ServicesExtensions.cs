namespace AdminIvoire.WebApi.BackgroundServices;

public static class ServicesExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<InitialisationDonneesLocalitePopulationService>();

        return services;
    }
}
