using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class VillageFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenVillageDoesNotExist_ThenAddVillage()
    {
        // Arrange
        var nom = "Abobo";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var sousPrefecture = new SousPrefecture { Nom = "Abobo", Departement= departement };
        var cancellationToken = CancellationToken.None;
        var villageWriteRepository = new Mock<IVillageWriteRepository>();
        var villageReadRepository = new Mock<IVillageReadRepository>();
        villageReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(Village));
        var sut = new VillageFactory(villageReadRepository.Object, villageWriteRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, sousPrefecture, cancellationToken);

        // Assert
        villageWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(sousPrefecture, result.SousPrefecture);
        Assert.Equal(population, result.Population);
    }

    [Fact]
    public async Task GivenGetOrCreateAsync_WhenVillageAlreadyExist_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Abobo";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var sousPrefecture = new SousPrefecture { Nom = "Abobo", Departement = departement };
        var cancellationToken = CancellationToken.None;
        const int initialPopulation = 1000000;
        var village = new Village { Nom = nom, Population = initialPopulation, SousPrefecture = sousPrefecture };
        var villageWriteRepository = new Mock<IVillageWriteRepository>();
        var villageReadRepository = new Mock<IVillageReadRepository>();
        villageReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(village);
        var sut = new VillageFactory(villageReadRepository.Object, villageWriteRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, sousPrefecture, cancellationToken);

        // Assert
        villageWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(sousPrefecture, result.SousPrefecture);
        Assert.Equal(population + initialPopulation, result.Population);
    }
}
