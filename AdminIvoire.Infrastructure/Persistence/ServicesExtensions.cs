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
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services
            .AddDbContext(configuration)
            .AddReadRepositories()
            .AddWriteRepositories();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LocaliteContext>(options => 
        { 
            options.UseNpgsql(configuration.GetConnectionString("LocaliteContext")); 
        });

        return services;
    }

    public static IServiceCollection AddReadRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICommuneReadRepository, CommuneReadRepository>();
        services.AddScoped<IDepartementReadRepository, DepartementReadRepository>();
        services.AddScoped<IDistrictReadRepository, DistrictReadRepository>();
        services.AddScoped<IRegionReadRepository, RegionReadRepository>();
        services.AddScoped<ISousPrefectureReadRepository, SousPrefectureReadRepository>();
        services.AddScoped<IVillageReadRepository, VillageReadRepository>();

        return services;
    }

    public static IServiceCollection AddWriteRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICommuneWriteRepository, CommuneWriteRepository>();
        services.AddScoped<IDepartementWriteRepository, DepartementWriteRepository>();
        services.AddScoped<IDistrictWriteRepository, DistrictWriteRepository>();
        services.AddScoped<IRegionWriteRepository, RegionWriteRepository>();
        services.AddScoped<ISousPrefectureWriteRepository, SousPrefectureWriteRepository>();
        services.AddScoped<IVillageWriteRepository, VillageWriteRepository>();

        return services;
    }
}