using AdminIvoire.Application.Command;
using AdminIvoire.Application.Parametrage;
using AdminIvoire.Application.Services;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.WebApi.BackgroundServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.WebApi.Tests.BackgroundServices;

public class AjoutVillagesAutomatiqueBackgroundServiceTests
{
    [Fact]
    public async Task GivenAjouterVillagesPourToutesSousPrefecturesAsync_WhenParametrageAlreadyExists_ThenDoNothing()
    {
        // Arrange
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(parametrageKey))
            .ReturnsAsync(new ParametrageEntity { Key = parametrageKey, Value = DateTime.UtcNow.ToString() });

        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var sut = new AjoutVillagesAutomatiqueBackgroundService(
            Mock.Of<ILogger<AjoutVillagesAutomatiqueBackgroundService>>(),
            serviceProvider);

        // Act
        await sut.AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken.None);

        // Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(parametrageKey), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.IsAny<ParametrageEntity>()), Times.Never);
    }

    [Fact]
    public async Task GivenAjouterVillagesPourToutesSousPrefecturesAsync_WhenNoSousPrefectures_ThenSetParametrage()
    {
        // Arrange
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(parametrageKey))
            .ReturnsAsync(default(ParametrageEntity));

        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var villageWebSourceServiceMock = new Mock<IVillageWebSourceService>();
        var senderMock = new Mock<ISender>();

        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(sousPrefectureReadRepositoryMock.Object);
        services.AddSingleton(villageWebSourceServiceMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var sut = new AjoutVillagesAutomatiqueBackgroundService(
            Mock.Of<ILogger<AjoutVillagesAutomatiqueBackgroundService>>(),
            serviceProvider);

        // Act
        await sut.AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken.None);

        // Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(parametrageKey), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == parametrageKey)), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        villageWebSourceServiceMock.Verify(x => x.GetVillagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        senderMock.Verify(x => x.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GivenAjouterVillagesPourToutesSousPrefecturesAsync_WhenSousPrefectureHasNoVillages_ThenSkipAndContinue()
    {
        // Arrange
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(parametrageKey))
            .ReturnsAsync(default(ParametrageEntity));

        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Test Sous-Prefecture",
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Test Departement",
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Test Region",
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Test District"
                    }
                }
            }
        };

        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([sousPrefecture]);

        var villageWebSourceServiceMock = new Mock<IVillageWebSourceService>();
        villageWebSourceServiceMock.Setup(x => x.GetVillagesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]); // Aucun village retourné

        var senderMock = new Mock<ISender>();

        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(sousPrefectureReadRepositoryMock.Object);
        services.AddSingleton(villageWebSourceServiceMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var sut = new AjoutVillagesAutomatiqueBackgroundService(
            Mock.Of<ILogger<AjoutVillagesAutomatiqueBackgroundService>>(),
            serviceProvider);

        // Act
        await sut.AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken.None);

        // Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(parametrageKey), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == parametrageKey)), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        villageWebSourceServiceMock.Verify(x => x.GetVillagesAsync(
            sousPrefecture.Nom,
            sousPrefecture.Departement.Nom,
            sousPrefecture.Departement.Region.Nom,
            It.IsAny<CancellationToken>()), Times.Once);
        senderMock.Verify(x => x.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GivenAjouterVillagesPourToutesSousPrefecturesAsync_WhenSousPrefectureHasVillages_ThenAddVillages()
    {
        // Arrange
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(parametrageKey))
            .ReturnsAsync(default(ParametrageEntity));

        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Test Sous-Prefecture",
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Test Departement",
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Test Region",
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Test District"
                    }
                }
            }
        };

        var villages = new List<string> { "Village1", "Village2" };

        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([sousPrefecture]);

        var villageWebSourceServiceMock = new Mock<IVillageWebSourceService>();
        villageWebSourceServiceMock.Setup(x => x.GetVillagesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(villages);

        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(sousPrefectureReadRepositoryMock.Object);
        services.AddSingleton(villageWebSourceServiceMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var sut = new AjoutVillagesAutomatiqueBackgroundService(
            Mock.Of<ILogger<AjoutVillagesAutomatiqueBackgroundService>>(),
            serviceProvider);

        // Act
        await sut.AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken.None);

        // Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(parametrageKey), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == parametrageKey)), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        villageWebSourceServiceMock.Verify(x => x.GetVillagesAsync(
            sousPrefecture.Nom,
            sousPrefecture.Departement.Nom,
            sousPrefecture.Departement.Region.Nom,
            It.IsAny<CancellationToken>()), Times.Once);
        senderMock.Verify(x => x.Send(
            It.Is<AjoutVillagesDeSousPrefecture.Command>(c =>
                c.SousPrefectureId == sousPrefecture.Id &&
                c.Villages.Length == villages.Count),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenAjouterVillagesPourToutesSousPrefecturesAsync_WhenErrorOccurs_ThenContinueAndSetParametrage()
    {
        // Arrange
        var parametrageKey = nameof(AjoutVillagesAutomatiqueBackgroundService);
        var parametrageRepositoryMock = new Mock<IParametrageRepository>();
        parametrageRepositoryMock.Setup(x => x.GetParametrageAsync(parametrageKey))
            .ReturnsAsync(default(ParametrageEntity));

        var sousPrefecture1 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Test Sous-Prefecture 1",
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Test Departement",
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Test Region",
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Test District"
                    }
                }
            }
        };

        var sousPrefecture2 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Test Sous-Prefecture 2",
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Test Departement 2",
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Test Region 2",
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Test District 2"
                    }
                }
            }
        };

        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([sousPrefecture1, sousPrefecture2]);

        var villageWebSourceServiceMock = new Mock<IVillageWebSourceService>();
        villageWebSourceServiceMock.Setup(x => x.GetVillagesAsync(
                sousPrefecture1.Nom,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test error"));
        villageWebSourceServiceMock.Setup(x => x.GetVillagesAsync(
                sousPrefecture2.Nom,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(["Village1"]);

        var senderMock = new Mock<ISender>();
        senderMock.Setup(x => x.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var services = new ServiceCollection();
        services.AddSingleton(parametrageRepositoryMock.Object);
        services.AddSingleton(sousPrefectureReadRepositoryMock.Object);
        services.AddSingleton(villageWebSourceServiceMock.Object);
        services.AddSingleton(senderMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var sut = new AjoutVillagesAutomatiqueBackgroundService(
            Mock.Of<ILogger<AjoutVillagesAutomatiqueBackgroundService>>(),
            serviceProvider);

        // Act
        await sut.AjouterVillagesPourToutesSousPrefecturesAsync(CancellationToken.None);

        // Assert
        parametrageRepositoryMock.Verify(x => x.GetParametrageAsync(parametrageKey), Times.Once);
        parametrageRepositoryMock.Verify(x => x.SetParametrageAsync(It.Is<ParametrageEntity>(p => p.Key == parametrageKey)), Times.Once);
        sousPrefectureReadRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        villageWebSourceServiceMock.Verify(x => x.GetVillagesAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        // Le deuxième appel devrait réussir malgré l'erreur du premier
        senderMock.Verify(x => x.Send(
            It.Is<AjoutVillagesDeSousPrefecture.Command>(c =>
                c.SousPrefectureId == sousPrefecture2.Id),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

