using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetAllSousPrefecturesHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllSousPrefectures()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetAllSousPrefectures.Handler>>();
        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        List<SousPrefecture> sousPrefectures = GetSousPrefecturesForTests();
        sousPrefectureReadRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(sousPrefectures);
        var handler = new GetAllSousPrefectures.Handler(loggerMock.Object, sousPrefectureReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetAllSousPrefectures.Query(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sousPrefectures.Count, result.Count());
        sousPrefectureReadRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    private static List<SousPrefecture> GetSousPrefecturesForTests()
    {
        var district1 = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 1",
            Superficie = 100,
            Population = 1000,
        };
        var region1 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district1,
        };
        var departement1 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 1",
            Superficie = 100,
            Population = 1000,
            Region = region1,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        var sousPrefecture1 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Sous-préfecture 1",
            Superficie = 100,
            Population = 1000,
            Departement = departement1,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        var sousPrefecture2 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Sous-préfecture 2",
            Superficie = 100,
            Population = 1000,
            Departement = departement1,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };

        return [sousPrefecture1, sousPrefecture2];
    }
}
