using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class CommuneFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenCommuneDoesNotExist_ThenAddCommune()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var cancellationToken = CancellationToken.None;
        var communeWriteRepository = new Mock<ICommuneWriteRepository>();
        var communeReadRepository = new Mock<ICommuneReadRepository>();
        communeReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(Commune));
        var sut = new CommuneFactory(communeWriteRepository.Object, communeReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, departement, cancellationToken);

        // Assert
        communeWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(departement, result.Departement);
        Assert.Equal(population, result.Population);
    }

    [Fact]
    public async Task GivenGetOrCreateAsync_WhenCommuneAlreadyExists_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var cancellationToken = CancellationToken.None;
        const int initialPopulation = 1000000;
        var commune = new Commune { Nom = nom, Departement = departement, Population = initialPopulation };
        var communeWriteRepository = new Mock<ICommuneWriteRepository>();
        var communeReadRepository = new Mock<ICommuneReadRepository>();
        communeReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(commune);
        var sut = new CommuneFactory(communeWriteRepository.Object, communeReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, departement, cancellationToken);

        // Assert
        communeWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(departement, result.Departement);
        Assert.Equal(population+initialPopulation, result.Population);
    }
}
