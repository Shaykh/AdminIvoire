using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class DistrictFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenDistrictDoesNotExist_ThenAddDistrict()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var cancellationToken = CancellationToken.None;
        var districtWriteRepository = new Mock<IDistrictWriteRepository>();
        var districtReadRepository = new Mock<IDistrictReadRepository>();
        districtReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(District));
        var sut = new DistrictFactory(districtWriteRepository.Object, districtReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, cancellationToken);
        // Assert
        districtWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(population, result.Population);
    }

    [Fact]
    public async Task GivenGetOrCreateAsync_WhenDistrictAlreadyExists_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var cancellationToken = CancellationToken.None;
        const int initialPopulation = 1000000;
        var district = new District { Nom = nom, Population = initialPopulation };
        var districtWriteRepository = new Mock<IDistrictWriteRepository>();
        var districtReadRepository = new Mock<IDistrictReadRepository>();
        districtReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(district);
        var sut = new DistrictFactory(districtWriteRepository.Object, districtReadRepository.Object);
        // Act
        var result = await sut.GetOrCreateAsync(nom, population, cancellationToken);
        // Assert
        districtWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(population + initialPopulation, result.Population);
    }
}
