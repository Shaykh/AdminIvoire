using AdminIvoire.Application.Command;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Factory;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Application;

/// <summary>
/// Extensions pour l'enregistrement des services de la couche Application
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Ajoute tous les services de la couche Application au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServicesExtensions).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assembly);
        });

        services.AddScoped<ILectureFichierCsvPopulationService, LectureFichierCsvPopulationService>();
        services.AddScoped<IVillageWebSourceService, VillageWebSourceService>();
        services.AddFactories();
        services.AddValidators();

        return services;
    }

    /// <summary>
    /// Ajoute les validateurs FluentValidation pour les commandes
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<AjoutVillagesDeSousPrefecture.Command>, AjoutVillagesDeSousPrefecture.Validator>();
        return services;
    }

    /// <summary>
    /// Ajoute les factories pour la création d'entités du domaine
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IDistrictFactory, DistrictFactory>();
        services.AddScoped<IRegionFactory, RegionFactory>();
        services.AddScoped<IDepartementFactory, DepartementFactory>();
        services.AddScoped<ISousPrefectureFactory, SousPrefectureFactory>();

        return services;
    }
}
