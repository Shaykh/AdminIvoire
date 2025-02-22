using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class RecupererCoordonneesGeographiquesDeVillagesHandlerTests
{
    [Fact]
    public async Task GivenHandler_WhenVillages_ThenCallGeocodingApiClientAndVillageWriteRepositoryAndReturnTrue()
    {
        // Arrange
        var sut = MakeSut(out var villageReadRepository, out var villageWriteRepository, out var geocodingApiClient, out var unitOfWork);
        var command = new RecupererCoordonneesGeographiquesDeVillages.Command();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 1,
            Longitude = 2
        };
        var listeNomVillages = new List<string> { "village1", "village2" };
        villageReadRepository.Setup(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(listeNomVillages);
        geocodingApiClient.Setup(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(coordonneesGeographiques);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        villageReadRepository.Verify(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        geocodingApiClient.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(listeNomVillages.Count));
        villageWriteRepository.Verify(x => x.UpdateCoordonneesGeographiquesAsync(It.IsAny<string>(), coordonneesGeographiques, It.IsAny<CancellationToken>()),
            Times.Exactly(listeNomVillages.Count));
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task GivenHandler_WhenNoVillage_ThenDONothingReturnFalse()
    {
        // Arrange
        var sut = MakeSut(out var villageReadRepository, out var villageWriteRepository, out var geocodingApiClient, out var unitOfWork);
        var command = new RecupererCoordonneesGeographiquesDeVillages.Command();
        var listeNomVillages = new List<string>();
        villageReadRepository.Setup(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(listeNomVillages);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        villageReadRepository.Verify(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        geocodingApiClient.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        villageWriteRepository.Verify(x => x.UpdateCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CoordonneesGeographiques>(), It.IsAny<CancellationToken>()),
            Times.Never);
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        Assert.False(result);
    }

    private static RecupererCoordonneesGeographiquesDeVillages.Handler MakeSut(
        out Mock<IVillageReadRepository> villageReadRepository,
        out Mock<IVillageWriteRepository> villageWriteRepository,
        out Mock<IGeocodingApiClient> geocodingApiClient,
        out Mock<IUnitOfWork> unitOfWork)
    {
        Mock<ILogger<RecupererCoordonneesGeographiquesDeVillages.Handler>> logger = new();
        villageReadRepository = new();
        villageWriteRepository = new();
        geocodingApiClient = new();
        unitOfWork = new();
        return new(logger.Object, villageReadRepository.Object, villageWriteRepository.Object, geocodingApiClient.Object, unitOfWork.Object);
    }
}
