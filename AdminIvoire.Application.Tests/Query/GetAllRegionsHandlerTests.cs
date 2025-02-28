using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetAllRegionsHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllRegions()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetAllRegions.Handler>>();
        var regionReadRepositoryMock = new Mock<IRegionReadRepository>();
        List<Region> regions = GetRegionsForTests();
        regionReadRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(regions);
        var handler = new GetAllRegions.Handler(loggerMock.Object, regionReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetAllRegions.Query(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(regions.Count, result.Count());
        regionReadRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    private static List<Region> GetRegionsForTests()
    {
        var district1 = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 1",
            Superficie = 100,
            Population = 1000,
        };
        var district2 = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 2",
            Superficie = 100,
            Population = 1000,
        };
        var region1 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district1
        };
        var region2 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 2",
            Superficie = 200,
            Population = 2000,
            District = district2
        };
        var region3 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 3",
            Superficie = 300,
            Population = 3000,
            District = district1
        };
        return [region1, region2, region3];
    }
}
