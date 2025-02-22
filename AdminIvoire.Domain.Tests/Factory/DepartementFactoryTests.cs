using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class DepartementFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenDepartementDoesNotExist_ThenAddDepartement()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var cancellationToken = CancellationToken.None;
        var departementWriteRepository = new Mock<IDepartementWriteRepository>();
        var departementReadRepository = new Mock<IDepartementReadRepository>();
        departementReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(Departement));
        var sut = new DepartementFactory(departementWriteRepository.Object, departementReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, region, cancellationToken);

        // Assert
        departementWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(region, result.Region);
        Assert.Equal(population, result.Population);
    }

    [Fact]
    public async Task GivenGetOrCreateAsync_WhenDepartementAlreadyExists_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        const int initialPopulation = 1000000;
        var departement = new Departement { Nom = "Abidjan", Region = region, Population = initialPopulation };
        var cancellationToken = CancellationToken.None;
        var departementWriteRepository = new Mock<IDepartementWriteRepository>();
        var departementReadRepository = new Mock<IDepartementReadRepository>();
        departementReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(departement);
        var sut = new DepartementFactory(departementWriteRepository.Object, departementReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, region, cancellationToken);

        // Assert
        departementWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(region, result.Region);
        Assert.Equal(population + initialPopulation, result.Population);
    }
}
