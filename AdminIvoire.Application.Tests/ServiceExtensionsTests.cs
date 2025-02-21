using AdminIvoire.Application.Services;
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
}
