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
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfDepartementAlreadyExists_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)))
            .ReturnsAsync(new ParametrageEntity { Key = nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements), Value = DateTime.UtcNow.ToString() });
        var senderMock = new Mock<ISender>();
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeDepartements.Command>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfDepartementDoesNotExistAndCommandResultTrue_ThenAddParametrageDepartement()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeDepartements.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeDepartements.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements))), Times.Once);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfDepartementDoesNotExistAndCommandResultFalse_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeDepartements.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeDepartements)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeDepartements.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.IsAny<ParametrageEntity>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfSousPrefectureAlreadyExists_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)))
            .ReturnsAsync(new ParametrageEntity { Key = nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures), Value = DateTime.UtcNow.ToString() });
        var senderMock = new Mock<ISender>();
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfSousPrefectureDoesNotExistAndCommandResultTrue_ThenAddParametrageSousPrefecture()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures))), Times.Once);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfSousPrefectureDoesNotExistAndCommandResultFalse_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeSousPrefectures)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeSousPrefectures.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.IsAny<ParametrageEntity>()), 
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfVillageAlreadyExists_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)))
            .ReturnsAsync(new ParametrageEntity { Key = nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages), Value = DateTime.UtcNow.ToString() });
        var senderMock = new Mock<ISender>();
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeVillages.Command>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfVillageDoesNotExistAndCommandResultTrue_ThenAddParametrageVillage()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeVillages.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeVillages.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages))), Times.Once);
    }

    [Fact]
    public async Task GivenRecupererDonneesLocaliteAsync_WhenParametrageOfVillageDoesNotExistAndCommandResultFalse_ThenDoNothing()
    {
        //Arrange
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)))
            .ReturnsAsync(default(ParametrageEntity));
        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeVillages.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        var sut = new RecuperationDonneesGeographiqueService(
            Mock.Of<ILogger<RecuperationDonneesGeographiqueService>>(),
            serviceProvider);

        //Act
        await sut.RecupererDonneesGeoLocaliteAsync(CancellationToken.None);

        //Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(nameof(RecuperationDonneesGeographiqueService) + nameof(RecupererCoordonneesGeographiquesDeVillages)), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<RecupererCoordonneesGeographiquesDeVillages.Command>(), It.IsAny<CancellationToken>()),
            Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.IsAny<ParametrageEntity>()),
            Times.Never);
    }
}
