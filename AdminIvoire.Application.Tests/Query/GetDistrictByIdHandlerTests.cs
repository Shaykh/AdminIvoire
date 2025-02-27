using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetDistrictByIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnDistrict()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetDistrictById.Handler>>();
        var districtReadRepositoryMock = new Mock<IDistrictReadRepository>();
        var district = GetDistrictForTests();
        districtReadRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(district);
        var handler = new GetDistrictById.Handler(loggerMock.Object, districtReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetDistrictById.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(district.Id, result.Id);
        Assert.Equal(district.Nom, result.Nom);
        Assert.Equal(district.Superficie, result.Superficie);
        Assert.Equal(district.Population, result.Population);
        Assert.Equal(district.Regions.Count, result.Regions.Count());
        districtReadRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    private static District GetDistrictForTests()
    {
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 1",
            Superficie = 100,
            Population = 1000,
        };
        district.Regions.Add(new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district
        });
        return district;
    }
}
