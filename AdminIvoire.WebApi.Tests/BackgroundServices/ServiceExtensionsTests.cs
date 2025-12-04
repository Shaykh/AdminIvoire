using AdminIvoire.WebApi.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdminIvoire.WebApi.Tests.BackgroundServices;

public class ServiceExtensionsTests
{
    [Fact]
    public void GivenServiceCollection_WhenAddBackgroundServices_ThenAddHostedServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddBackgroundServices();

        // Assert
        Assert.Contains(services, x => x.ServiceType == typeof(IHostedService) && x.ImplementationType == typeof(InitialisationDonneesLocalitePopulationBackgroundService));
        Assert.Contains(services, x => x.ServiceType == typeof(IHostedService) && x.ImplementationType == typeof(RecuperationDonneesGeographiqueBackgroundService));
        Assert.Contains(services, x => x.ServiceType == typeof(IHostedService) && x.ImplementationType == typeof(AjoutVillagesAutomatiqueBackgroundService));
    }
}
