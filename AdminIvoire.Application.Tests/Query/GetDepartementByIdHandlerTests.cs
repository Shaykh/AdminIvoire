using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetDepartementByIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnDepartement()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetDepartementById.Handler>>();
        var departementReadRepositoryMock = new Mock<IDepartementReadRepository>();
        var departement = GetDepartementForTests();
        departementReadRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(departement);
        var handler = new GetDepartementById.Handler(loggerMock.Object, departementReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetDepartementById.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departement.Id, result.Id);
        Assert.Equal(departement.Nom, result.Nom);
        Assert.Equal(departement.Superficie, result.Superficie);
        Assert.Equal(departement.Population, result.Population);
        Assert.Equal(departement.Region.Id, result.RegionId);
        Assert.Equal(departement.Region.Nom, result.RegionNom);
        Assert.Equal(departement.Region.District.Nom, result.DistrictNom);
        departementReadRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }
    private static Departement GetDepartementForTests()
    {
        var district1 = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 1",
            Superficie = 100,
            Population = 1000,
        };
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district1,
            DistrictId = district1.Id
        };
        var departement = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 1",
            Superficie = 100,
            Population = 1000,
            Region = region,
            RegionId = region.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return departement;
    }
}
