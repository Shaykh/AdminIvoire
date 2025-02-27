using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetAllDistrictsHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllDistricts()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetAllDistricts.Handler>>();
        var districtReadRepositoryMock = new Mock<IDistrictReadRepository>();
        IList<District> districts = GetDistrictForTests();
        districtReadRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(districts);
        var handler = new GetAllDistricts.Handler(loggerMock.Object, districtReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetAllDistricts.Query(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(districts.Count, result.Count());
        districtReadRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    private static IList<District> GetDistrictForTests()
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
            Superficie = 200,
            Population = 2000
        };
        var district3 = new District
        {
            Id = Guid.NewGuid(),
            Nom = "District 3",
            Superficie = 300,
            Population = 3000
        };
        district1.Regions.Add(new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 1",
            Superficie = 100,
            Population = 1000,
            District = district1
        });
        district2.Regions.Add(new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 2",
            Superficie = 200,
            Population = 2000,
            District = district2
        });
        district3.Regions.Add(new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 3",
            Superficie = 300,
            Population = 3000,
            District = district3
        });
        district1.Regions.Add(new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 4",
            Superficie = 400,
            Population = 4000,
            District = district1
        });
        IList<District> districts = [district1, district2, district3];
        return districts;
    }
}
