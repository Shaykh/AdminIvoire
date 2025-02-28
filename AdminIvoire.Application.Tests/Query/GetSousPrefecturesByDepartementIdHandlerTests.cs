using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetSousPrefecturesByDepartementIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnSousPrefecturesOfDepartement()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetSousPrefecturesByDepartementId.Handler>>();
        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        List<SousPrefecture> sousPrefectures = GetSousPrefecturesForTests();
        sousPrefectureReadRepositoryMock.Setup(r => r.GetAllByDepartementIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(sousPrefectures);
        var handler = new GetSousPrefecturesByDepartementId.Handler(loggerMock.Object, sousPrefectureReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetSousPrefecturesByDepartementId.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sousPrefectures.Count, result.Count());
        sousPrefectureReadRepositoryMock.Verify(r => r.GetAllByDepartementIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    private static List<SousPrefecture> GetSousPrefecturesForTests()
    {
        var departement1 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 1",
            Superficie = 100,
            Population = 1000,
            Region = new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Region 1",
                Superficie = 100,
                Population = 1000,
                District = new District
                {
                    Id = Guid.NewGuid(),
                    Nom = "District 1",
                    Superficie = 100,
                    Population = 1000,
                },
                DistrictId = Guid.NewGuid()
            },
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        var sousPrefecture1 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "SousPrefecture 1",
            Superficie = 100,
            Population = 1000,
            Departement = departement1,
            DepartementId = departement1.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        var sousPrefecture2 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "SousPrefecture 2",
            Superficie = 100,
            Population = 1000,
            Departement = departement1,
            DepartementId = departement1.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return [sousPrefecture1, sousPrefecture2];
    }
}
