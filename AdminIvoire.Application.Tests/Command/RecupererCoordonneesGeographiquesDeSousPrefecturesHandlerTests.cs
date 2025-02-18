using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Repository;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Command;

public class RecupererCoordonneesGeographiquesDeSousPrefecturesHandlerTests
{
    [Fact]
    public async Task GivenHandler_WhenHandle_ThenShouldCallGeocodingApiClientAndSousPrefectureWriteRepository()
    {
        // Arrange
        var sut = MakeSut(out var sousPrefectureReadRepository, out var sousPrefectureWriteRepository, out var geocodingApiClient, out var unitOfWork);
        var command = new RecupererCoordonneesGeographiquesDeSousPrefectures.Command();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 1,
            Longitude = 2
        };
        var listeNomSousPrefectures = new List<string> { "nom1", "nom2" };
        sousPrefectureReadRepository.Setup(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(listeNomSousPrefectures);
        geocodingApiClient.Setup(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(coordonneesGeographiques);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        sousPrefectureReadRepository.Verify(x => x.GetAllNomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        geocodingApiClient.Verify(x => x.GetCoordonneesGeographiquesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(listeNomSousPrefectures.Count));
        sousPrefectureWriteRepository.Verify(x => x.UpdateCoordonneesGeographiquesAsync(It.IsAny<string>(), coordonneesGeographiques, It.IsAny<CancellationToken>()), 
            Times.Exactly(listeNomSousPrefectures.Count));
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static RecupererCoordonneesGeographiquesDeSousPrefectures.Handler MakeSut(
        out Mock<ISousPrefectureReadRepository> sousPrefectureReadRepository,
        out Mock<ISousPrefectureWriteRepository> sousPrefectureWriteRepository,
        out Mock<IGeocodingApiClient> geocodingApiClient,
        out Mock<IUnitOfWork> unitOfWork)
    {
        Mock<ILogger<RecupererCoordonneesGeographiquesDeSousPrefectures.Handler>> logger = new();
        sousPrefectureReadRepository = new();
        sousPrefectureWriteRepository = new();
        geocodingApiClient = new();
        unitOfWork = new();
        return new(logger.Object, sousPrefectureReadRepository.Object, sousPrefectureWriteRepository.Object, geocodingApiClient.Object, unitOfWork.Object);
    }
}
