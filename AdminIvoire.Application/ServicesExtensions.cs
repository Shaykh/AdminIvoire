using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Application;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServicesExtensions).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assembly);
        });

        services.AddScoped<ILectureFichierCsvPopulationService, LectureFichierCsvPopulationService>();
        services.AddFactories();

        return services;
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IDistrictFactory, DistrictFactory>();
        services.AddScoped<IRegionFactory, RegionFactory>();
        services.AddScoped<IDepartementFactory, DepartementFactory>();
        services.AddScoped<ISousPrefectureFactory, SousPrefectureFactory>();
        services.AddScoped<ICommuneFactory, CommuneFactory>();

        return services;
    }
}
