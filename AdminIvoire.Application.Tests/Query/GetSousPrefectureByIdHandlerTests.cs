using AdminIvoire.Application.Query;
using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.ValueObject;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Query;

public class GetSousPrefectureByIdHandlerTests
{
    [Fact]
    public async Task GivenHandle_WhenCalled_ThenReturnSousPrefecture()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GetSousPrefectureById.Handler>>();
        var sousPrefectureReadRepositoryMock = new Mock<ISousPrefectureReadRepository>();
        var sousPrefecture = GetSousPrefectureForTests();
        sousPrefectureReadRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(sousPrefecture);
        var handler = new GetSousPrefectureById.Handler(loggerMock.Object, sousPrefectureReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new GetSousPrefectureById.Query(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sousPrefecture.Id, result.Id);
        Assert.Equal(sousPrefecture.Nom, result.Nom);
        Assert.Equal(sousPrefecture.Superficie, result.Superficie);
        Assert.Equal(sousPrefecture.Population, result.Population);
        Assert.Equal(sousPrefecture.Departement.Id, result.DepartementId);
        Assert.Equal(sousPrefecture.Departement.Nom, result.DepartementNom);
        Assert.Equal(sousPrefecture.Departement.Region.Nom, result.RegionNom);
        Assert.Equal(sousPrefecture.Departement.Region.District.Nom, result.DistrictNom);
        sousPrefectureReadRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    private static SousPrefecture GetSousPrefectureForTests()
    {
        var departement = new Departement
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
            RegionId = Guid.NewGuid()
        };
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Sous-préfecture 1",
            Superficie = 100,
            Population = 1000,
            Departement = departement,
            DepartementId = departement.Id,
            CoordonneesGeographiques = new CoordonneesGeographiques
            {
                Latitude = 1.0m,
                Longitude = 1.0m
            }
        };
        return sousPrefecture;
    }
}
