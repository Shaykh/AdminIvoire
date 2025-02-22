using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class RegionFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenRegionDoesNotExist_ThenAddRegion()
    {
        // Arrange
        var nom = "Lagunes";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var cancellationToken = CancellationToken.None;
        var regionWriteRepository = new Mock<IRegionWriteRepository>();
        var regionReadRepository = new Mock<IRegionReadRepository>();
        regionReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(Region));
        var sut = new RegionFactory(regionReadRepository.Object, regionWriteRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, district, cancellationToken);

        // Assert
        regionWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(district, result.District);
        Assert.Equal(population, result.Population);
    }
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenRegionAlreadyExists_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Lagunes";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var cancellationToken = CancellationToken.None;
        const int initialPopulation = 1000000;
        var region = new Region { Nom = nom, District = district, Population = initialPopulation };
        var regionWriteRepository = new Mock<IRegionWriteRepository>();
        var regionReadRepository = new Mock<IRegionReadRepository>();
        regionReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(region);
        var sut = new RegionFactory(regionReadRepository.Object, regionWriteRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, district, cancellationToken);

        // Assert
        regionWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(district, result.District);
        Assert.Equal(population + initialPopulation, result.Population);
    }
}
