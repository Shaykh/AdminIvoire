using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetAllDepartementsHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnAllDepartements()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetAllDepartements.Handler>>();
        var departementReadRepositoryMock = new Mock<IDepartementReadRepository>();
        List<Departement> departements = GetDepartementsForTests();
        departementReadRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(departements);
        var handler = new GetAllDepartements.Handler(loggerMock.Object, departementReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetAllDepartements.Query(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departements.Count, result.Count());
        departementReadRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    private static List<Departement> GetDepartementsForTests()
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
            District = district1,
        };
        var region2 = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Region 2",
            Superficie = 200,
            Population = 2000,
            District = district2,
        };
        var departement1 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 1",
            Superficie = 100,
            Population = 1000,
            Region = region1
        };
        var departement2 = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Departement 2",
            Superficie = 200,
            Population = 2000,
            Region = region2
        };
        return [departement1, departement2];
    }
}
