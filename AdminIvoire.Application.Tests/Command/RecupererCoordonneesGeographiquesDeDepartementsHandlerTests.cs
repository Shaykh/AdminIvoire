using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class RecupererCoordonneesGeographiquesDeDepartementsHandlerTests
{
    [Fact]
    public async Task GivenHandler_WhenDepartements_ThenCallGeocodingApiClientAndDepartementWriteRepositoryAndReturnTrue()
    {
        // Arrange
        var sut = MakeSut(out var departementReadRepository, out var departementWriteRepository, out var geocodingApiClient, out var unitOfWork);
        var command = new RecupererCoordonneesGeographiquesDeDepartements.Command();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 1,
            Longitude = 2
        };
        var listeNomDepartements = new List<string> { "departement1", "departement2" };
        departementReadRepository.Setup(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(listeNomDepartements);
        geocodingApiClient.Setup(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(coordonneesGeographiques);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        departementReadRepository.Verify(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        geocodingApiClient.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(listeNomDepartements.Count));
        departementWriteRepository.Verify(x => x.UpdateCoordonneesGeographiquesAsync(It.IsAny<string>(), coordonneesGeographiques, It.IsAny<CancellationToken>()),
            Times.Exactly(listeNomDepartements.Count));
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task GivenHandler_WhenNoDepartement_ThenDONothingReturnFalse()
    {
        // Arrange
        var sut = MakeSut(out var departementReadRepository, out var departementWriteRepository, out var geocodingApiClient, out var unitOfWork);
        var command = new RecupererCoordonneesGeographiquesDeDepartements.Command();
        var listeNomDepartements = new List<string>();
        departementReadRepository.Setup(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(listeNomDepartements);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        departementReadRepository.Verify(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        geocodingApiClient.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        departementWriteRepository.Verify(x => x.UpdateCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CoordonneesGeographiques>(), It.IsAny<CancellationToken>()),
            Times.Never);
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        Assert.False(result);
    }

    private static RecupererCoordonneesGeographiquesDeDepartements.Handler MakeSut(
        out Mock<IDepartementReadRepository> departementReadRepository,
        out Mock<IDepartementWriteRepository> departementWriteRepository,
        out Mock<IGeocodingApiClient> geocodingApiClient,
        out Mock<IUnitOfWork> unitOfWork)
    {
        Mock<ILogger<RecupererCoordonneesGeographiquesDeDepartements.Handler>> logger = new();
        departementReadRepository = new();
        departementWriteRepository = new();
        geocodingApiClient = new();
        unitOfWork = new();
        return new(logger.Object, departementReadRepository.Object, departementWriteRepository.Object, geocodingApiClient.Object, unitOfWork.Object);
    }
}
