using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetDepartementsByRegionIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllDepartementsOfRegion()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetDepartementsByRegionId.Handler>>();
        var departementReadRepositoryMock = new Mock<IDepartementReadRepository>();
        var regionId = Guid.NewGuid();
        List<Departement> departements = GetDepartementsForTests(regionId);
        departementReadRepositoryMock.Setup(r => r.GetAllByRegionIdAsync(regionId, It.IsAny<CancellationToken>())).ReturnsAsync(departements);
        var handler = new GetDepartementsByRegionId.Handler(loggerMock.Object, departementReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetDepartementsByRegionId.Query(regionId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departements.Count, result.Count());
        departementReadRepositoryMock.Verify(r => r.GetAllByRegionIdAsync(regionId, CancellationToken.None), Times.Once);
    }

    private static List<Departement> GetDepartementsForTests(Guid regionId)
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
            Id = regionId,
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district1
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
        var departement2 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 2",
            Superficie = 200,
            Population = 2000,
            Region = region1,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 2.0m,
                Longitude = 2.0m
            }
        };
        var departement3 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 3",
            Superficie = 300,
            Population = 3000,
            Region = region1,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 3.0m,
                Longitude = 3.0m
            }
        };
        return [departement1, departement2, departement3];
    }
}
