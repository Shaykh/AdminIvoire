using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Infrastructure.Configuration;
using AdminIvoire.WebApi.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.WebApi.Tests.BackgroundServices;

public class SeedLocalitePopulationServiceTests
{
    [Fact]
    public async Task GivenReadLocaliteDataAsync_WhenNoFileConfiguration_ThenThrowsException()
    {
        //Arrange
        var sut = new SeedLocalitePopulationService(
            Mock.Of<ILogger<SeedLocalitePopulationService>>(),
            Mock.Of<IServiceProvider>(),
            new ConfigurationBuilder().Build(),
            Mock.Of<IWebHostEnvironment>());

        //Act
        async Task act() => await sut.ReadLocaliteDataAsync(CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ConfigurationException>(act);
    }

    [Fact]
    public async Task GivenReadLocaliteDataAsync_WhenFileConfigurationExistsAndParametrageAlreadyExists_ThenDoNothing()
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new ("FichierPopulation", "data.csv")
            ])
            .Build();
        var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(SeedLocalitePopulationService)))
            .ReturnsAsync(new ParametrageEntity { Key = nameof(SeedLocalitePopulationService), Value = DateTime.UtcNow.ToString() });
        var lectureFichierCsvPopulationServiceMock = new Mock<ILectureFichierCsvPopulationService>();
        lectureFichierCsvPopulationServiceMock.Setup(x => x.LireFichierCsvPopulationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(lectureFichierCsvPopulationServiceMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new SeedLocalitePopulationService(
            Mock.Of<ILogger<SeedLocalitePopulationService>>(),
            serviceProvider,
            configuration,
            webHostEnvironmentMock.Object);

        //Act
        await sut.ReadLocaliteDataAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(SeedLocalitePopulationService)), Times.Once);
        lectureFichierCsvPopulationServiceMock.Verify(x => x.LireFichierCsvPopulationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task GivenReadLocaliteDataAsync_WhenFileConfigurationExistsAndParametrageDoesNotExist_ThenLireFichier()
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([
                new ("FichierPopulation", "data.csv")
            ])
            .Build();
        var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(SeedLocalitePopulationService)))
            .ReturnsAsync(default(ParametrageEntity));
        var lectureFichierCsvPopulationServiceMock = new Mock<ILectureFichierCsvPopulationService>();
        lectureFichierCsvPopulationServiceMock.Setup(x => x.LireFichierCsvPopulationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(lectureFichierCsvPopulationServiceMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new SeedLocalitePopulationService(
            Mock.Of<ILogger<SeedLocalitePopulationService>>(),
            serviceProvider,
            configuration,
            webHostEnvironmentMock.Object);

        //Act
        await sut.ReadLocaliteDataAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(SeedLocalitePopulationService)), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == nameof(SeedLocalitePopulationService))), Times.Once);
        lectureFichierCsvPopulationServiceMock.Verify(x => x.LireFichierCsvPopulationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
