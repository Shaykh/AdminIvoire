using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetAllVillagesHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllVillages()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetAllVillages.Handler>>();
        var villageReadRepositoryMock = new Mock<IVillageReadRepository>();
        List<Village> villages = GetVillagesForTests();
        villageReadRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(villages);
        var handler = new GetAllVillages.Handler(loggerMock.Object, villageReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetAllVillages.Query(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(villages.Count, result.Count());
        villageReadRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    private static List<Village> GetVillagesForTests()
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
        var village1 = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Village 1",
            Superficie = 100,
            Population = 1000,
            SousPrefecture = sousPrefecture1,
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
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return [village1, village2];
    }
}
