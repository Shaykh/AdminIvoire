using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetVillagesBySousPrefectureIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnVillagesOfSousPrefecture()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetVillagesBySousPrefectureId.Handler>>();
        var villageReadRepositoryMock = new Mock<IVillageReadRepository>();
        List<Village> villages = GetVillagesForTests();
        villageReadRepositoryMock.Setup(r => r.GetAllBySousPrefectureIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(villages);
        var handler = new GetVillagesBySousPrefectureId.Handler(loggerMock.Object, villageReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetVillagesBySousPrefectureId.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(villages.Count, result.Count());
        villageReadRepositoryMock.Verify(r => r.GetAllBySousPrefectureIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    private static List<Village> GetVillagesForTests()
    {
        var sousPrefecture1 = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "SousPrefecture 1",
            Superficie = 100,
            Population = 1000,
            Departement = new Departement
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
            },
            DepartementId = Guid.NewGuid()
        };
        var village1 = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Village 1",
            Superficie = 100,
            Population = 1000,
            SousPrefecture = sousPrefecture1,
            SousPrefectureId = sousPrefecture1.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        var village2 = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Village 2",
            Superficie = 100,
            Population = 1000,
            SousPrefecture = sousPrefecture1,
            SousPrefectureId = sousPrefecture1.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return [village1, village2];
    }
}
