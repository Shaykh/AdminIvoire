using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Parametrage;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.Tests;

public class ServiceExtensionsTests
{
    [Fact]
    public void GivenServiceCollection_WhenAddInfrastructure_ThenAddLocaliteContextAndParametreRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new KeyValuePair<string, string?>("LocaliteContext", "Server=localhost;Port=5432;Database=localite;User Id=postgres;Password=;"),
                ])
            .Build();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(LocaliteContext) && x.ImplementationType == typeof(LocaliteContext));
        Assert.Contains(services, x => x.ServiceType == typeof(IParametrageRepository) && x.ImplementationType == typeof(ParametrageRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IUnitOfWork) && x.ImplementationType == typeof(UnitOfWork));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddInfrastructure_ThenAddReadRepositories()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new KeyValuePair<string, string?>("LocaliteContext", "Server=localhost;Port=5432;Database=localite;User Id=postgres;Password=;"),
                ])
            .Build();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IVillageReadRepository) && x.ImplementationType == typeof(VillageReadRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(ISousPrefectureReadRepository) && x.ImplementationType == typeof(SousPrefectureReadRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IDepartementReadRepository) && x.ImplementationType == typeof(DepartementReadRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IRegionReadRepository) && x.ImplementationType == typeof(RegionReadRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IDistrictReadRepository) && x.ImplementationType == typeof(DistrictReadRepository));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddInfrastructure_ThenAddWriteRepositories()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new KeyValuePair<string, string?>("LocaliteContext", "Server=localhost;Port=5432;Database=localite;User Id=postgres;Password=;"),
                ])
            .Build();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IVillageWriteRepository) && x.ImplementationType == typeof(VillageWriteRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(ISousPrefectureWriteRepository) && x.ImplementationType == typeof(SousPrefectureWriteRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IDepartementWriteRepository) && x.ImplementationType == typeof(DepartementWriteRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IRegionWriteRepository) && x.ImplementationType == typeof(RegionWriteRepository));
        Assert.Contains(services, x => x.ServiceType == typeof(IDistrictWriteRepository) && x.ImplementationType == typeof(DistrictWriteRepository));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddInfrastructure_ThenAddIGeocodingApiClient()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new KeyValuePair<string, string?>("LocaliteContext", "Server=localhost;Port=5432;Database=localite;User Id=postgres;Password=;"),
                ])
            .Build();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IGeocodingApiClient));
    }
}
