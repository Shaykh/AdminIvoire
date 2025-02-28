using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetVillageByIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnVillage()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetVillageById.Handler>>();
        var villageReadRepositoryMock = new Mock<IVillageReadRepository>();
        var village = GetVillageForTests();
        villageReadRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(village);
        var handler = new GetVillageById.Handler(loggerMock.Object, villageReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetVillageById.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(village.Id, result.Id);
        Assert.Equal(village.Nom, result.Nom);
        Assert.Equal(village.Superficie, result.Superficie);
        Assert.Equal(village.Population, result.Population);
        Assert.Equal(village.SousPrefecture!.Id, result.SousPrefectureId);
        Assert.Equal(village.SousPrefecture.Nom, result.SousPrefectureNom);
        Assert.Equal(village.SousPrefecture.Departement.Nom, result.DepartementNom);
        Assert.Equal(village.SousPrefecture.Departement.Region.Nom, result.RegionNom);
        Assert.Equal(village.SousPrefecture.Departement.Region.District.Nom, result.DistrictNom);
    }

    private static Village GetVillageForTests()
    {
        var sousPrefecture = new SousPrefecture
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
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Village 1",
            Superficie = 100,
            Population = 1000,
            SousPrefecture = sousPrefecture,
            SousPrefectureId = sousPrefecture.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return village;
    }
}
