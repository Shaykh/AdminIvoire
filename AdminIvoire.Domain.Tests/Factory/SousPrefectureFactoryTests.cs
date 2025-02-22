using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Factory;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using Moq;

namespace AdminIvoire.Domain.Tests.Factory;

public class SousPrefectureFactoryTests
{
    [Fact]
    public async Task GivenGetOrCreateAsync_WhenSousPrefectureDoesNotExist_ThenAddSousPrefecture()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var cancellationToken = CancellationToken.None;
        var sousPrefectureWriteRepository = new Mock<ISousPrefectureWriteRepository>();
        var sousPrefectureReadRepository = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(default(SousPrefecture));
        var sut = new SousPrefectureFactory(sousPrefectureWriteRepository.Object, sousPrefectureReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, departement, cancellationToken);

        // Assert
        sousPrefectureWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Once);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(departement, result.Departement);
        Assert.Equal(population, result.Population);
    }

    [Fact]
    public async Task GivenGetOrCreateAsync_WhenSousPrefectureAlreadyExists_ThenUpdatePopulation()
    {
        // Arrange
        var nom = "Abidjan";
        var population = 5000000;
        var district = new District { Nom = "Abidjan" };
        var region = new Region { Nom = "Lagunes", District = district };
        var departement = new Departement { Nom = "Lagunes", Region = region };
        var cancellationToken = CancellationToken.None;
        const int initialPopulation = 1000000;
        var sousPrefecture = new SousPrefecture { Nom = nom, Departement = departement, Population = initialPopulation };
        var sousPrefectureWriteRepository = new Mock<ISousPrefectureWriteRepository>();
        var sousPrefectureReadRepository = new Mock<ISousPrefectureReadRepository>();
        sousPrefectureReadRepository.Setup(x => x.GetByNomAsync(nom, cancellationToken)).ReturnsAsync(sousPrefecture);
        var sut = new SousPrefectureFactory(sousPrefectureWriteRepository.Object, sousPrefectureReadRepository.Object);

        // Act
        var result = await sut.GetOrCreateAsync(nom, population, departement, cancellationToken);

        // Assert
        sousPrefectureWriteRepository.Verify(x => x.AddAsync(result, cancellationToken), Times.Never);
        Assert.Equal(nom, result.Nom);
        Assert.Equal(departement, result.Departement);
        Assert.Equal(population + initialPopulation, result.Population);
    }
}
