using AdminIvoire.Application.Command;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Factory;
using FluentValidation;
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
        services.AddValidators();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<AjoutVillagesDeSousPrefecture.Command>, AjoutVillagesDeSousPrefecture.Validator>();
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
