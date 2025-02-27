using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetRegionsByDistrictIdTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllRegionsOfDistrict()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetRegionsByDistrictId.Handler>>();
        var regionReadRepositoryMock = new Mock<IRegionReadRepository>();
        var ditrictId = Guid.NewGuid();
        List<Region> regions = GetRegionsForTests(ditrictId);
        regionReadRepositoryMock.Setup(r => r.GetAllByDistrictIdAsync(ditrictId, It.IsAny<CancellationToken>())).ReturnsAsync(regions);
        var handler = new GetRegionsByDistrictId.Handler(loggerMock.Object, regionReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetRegionsByDistrictId.Query(ditrictId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(regions.Count, result.Count());
        regionReadRepositoryMock.Verify(r => r.GetAllByDistrictIdAsync(ditrictId, CancellationToken.None), Times.Once);
    }

    private static List<Region> GetRegionsForTests(Guid districtId)
    {
        var district1 = new District
        {
            Id = districtId,
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
            District = district1
        };
        var region2 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 2",
            Superficie = 200,
            Population = 2000,
            District = district1
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
