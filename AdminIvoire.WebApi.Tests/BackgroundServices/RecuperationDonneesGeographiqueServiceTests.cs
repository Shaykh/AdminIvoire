using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using AdminIvoire.WebApi.BackgroundServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.WebApi.Tests.BackgroundServices;

public class RecuperationDonneesGeographiqueServiceTests
{
    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageAlreadyExists_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService)))
            .ReturnsAsync(new ParametrageEntity { Key = nameof(RecuperationDonneesGeographiqueService), Value = DateTime.UtcNow.ToString() });
        var senderMock = new Mock<ISender>();
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageDoesNotExist_ThenSendCommand()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
