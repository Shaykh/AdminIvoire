using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetRegionByIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnRegion()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetRegionById.Handler>>();
        var regionReadRepositoryMock = new Mock<IRegionReadRepository>();
        var region = GetRegionForTests();
        regionReadRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(region);
        var handler = new GetRegionById.Handler(loggerMock.Object, regionReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetRegionById.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(region.Id, result.Id);
        Assert.Equal(region.Nom, result.Nom);
        Assert.Equal(region.Superficie, result.Superficie);
        Assert.Equal(region.Population, result.Population);
        Assert.Equal(region.Departements.Count, result.Departements.Count());
        regionReadRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }
    private static Region GetRegionForTests()
    {
        var district = new District
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
            District = district
        };
        region.Departements.Add(new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 1",
            Superficie = 100,
            Population = 1000,
            Region = region
        });
        region.Departements.Add(new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 2",
            Superficie = 200,
            Population = 2000,
            Region = region
        });
        return region;
    }
}
