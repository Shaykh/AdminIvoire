using AdminIvoire.Application.Command;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Factory;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Application.Tests;

public class ServiceExtensionsTests
{
    [Fact]
    public void GivenServiceCollection_WhenAddApplication_ThenAddMediatRAndLectureFichierCsvPopulationService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(ILectureFichierCsvPopulationService) && x.ImplementationType == typeof(LectureFichierCsvPopulationService));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddApplication_ThenAddMediatR()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IMediator) && x.ImplementationType == typeof(Mediator));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddApplication_ThenAddFactories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IDistrictFactory) && x.ImplementationType == typeof(DistrictFactory));
        Assert.Contains(services, x => x.ServiceType == typeof(IRegionFactory) && x.ImplementationType == typeof(RegionFactory));
        Assert.Contains(services, x => x.ServiceType == typeof(IDepartementFactory) && x.ImplementationType == typeof(DepartementFactory));
        Assert.Contains(services, x => x.ServiceType == typeof(ISousPrefectureFactory) && x.ImplementationType == typeof(SousPrefectureFactory));
        Assert.Contains(services, x => x.ServiceType == typeof(ICommuneFactory) && x.ImplementationType == typeof(CommuneFactory));
    }

    [Fact]
    public void GivenServiceCollection_WhenAddApplication_ThenAddValidators()
    {
        // Arrange
        var services = new ServiceCollection();
        // Act
        services.AddApplication();
        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IValidator<AjoutVillagesDeSousPrefecture.Command>) && x.ImplementationType == typeof(AjoutVillagesDeSousPrefecture.Validator));
    }
}
