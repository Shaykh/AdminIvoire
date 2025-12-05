using AdminIvoire.Application.Parametrage;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using AdminIvoire.Infrastructure.Persistence.Repository;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.Persistence;

public static class ServicesExtensions
{
    /// <summary>
    /// Effectue l'injection de dépendance des services de persistence de données
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <param name="configuration">La configuration de l'application</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services
            .AddLocaliteDbContext(configuration)
            .AddScoped<IParametrageRepository, ParametrageRepository>()
            .AddReadRepositories()
            .AddWriteRepositories();

        return services;
    }

    /// <summary>
    /// Ajoute le DbContext LocaliteContext au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <param name="configuration">La configuration de l'application</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddLocaliteDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LocaliteContext>(options => 
        { 
            options.UseNpgsql(configuration.GetConnectionString("LocaliteContext")); 
        });

        return services;
    }

    /// <summary>
    /// Ajoute tous les repositories de lecture au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddReadRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDepartementReadRepository, DepartementReadRepository>();
        services.AddScoped<IDistrictReadRepository, DistrictReadRepository>();
        services.AddScoped<IRegionReadRepository, RegionReadRepository>();
        services.AddScoped<ISousPrefectureReadRepository, SousPrefectureReadRepository>();
        services.AddScoped<IVillageReadRepository, VillageReadRepository>();

        return services;
    }

    /// <summary>
    /// Ajoute tous les repositories d'écriture au conteneur d'injection de dépendances
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour permettre le chaînage</returns>
    public static IServiceCollection AddWriteRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDepartementWriteRepository, DepartementWriteRepository>();
        services.AddScoped<IDistrictWriteRepository, DistrictWriteRepository>();
        services.AddScoped<IRegionWriteRepository, RegionWriteRepository>();
        services.AddScoped<ISousPrefectureWriteRepository, SousPrefectureWriteRepository>();
        services.AddScoped<IVillageWriteRepository, VillageWriteRepository>();

        return services;
    }
}