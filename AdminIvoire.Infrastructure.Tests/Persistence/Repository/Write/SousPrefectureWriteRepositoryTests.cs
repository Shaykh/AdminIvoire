using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class SousPrefectureWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidSousPrefecture_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidSousPrefecture_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        await sut.AddAsync(sousPrefecture, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.SousPrefectures);
        Assert.Equal(sousPrefecture.Id, persisted.Id);
        Assert.Equal(sousPrefecture.Nom, persisted.Nom);
        Assert.Equal(sousPrefecture.DepartementId, persisted.DepartementId);
        Assert.Equal(sousPrefecture.Population, persisted.Population);
        Assert.Equal(sousPrefecture.Superficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingSousPrefecture_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingSousPrefecture_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        sousPrefecture.Population = updatedPopulation;
        sousPrefecture.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(sousPrefecture, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.SousPrefectures);
        Assert.Equal(sousPrefecture.Id, persisted.Id);
        Assert.Equal(sousPrefecture.Nom, persisted.Nom);
        Assert.Equal(sousPrefecture.DepartementId, persisted.DepartementId);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingSousPrefecture_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingSousPrefecture_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            }
        };

        // Act
        async Task act() => await sut.UpdateAsync(sousPrefecture, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenExistingSousPrefecture_ThenUpdateSuperficie()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenExistingSousPrefecture_ThenUpdateSuperficie))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();
        const decimal updatedSuperficie = 354m;

        // Act
        await sut.UpdateSuperficieAsync(sousPrefecture.Nom, updatedSuperficie, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.SousPrefectures);
        Assert.Equal(sousPrefecture.Id, persisted.Id);
        Assert.Equal(sousPrefecture.Nom, persisted.Nom);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenNoExistingSousPrefecture_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenNoExistingSousPrefecture_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            }
        };

        // Act
        async Task act() => await sut.UpdateSuperficieAsync(sousPrefecture.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenExistingSousPrefecture_ThenUpdatePopulation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenExistingSousPrefecture_ThenUpdatePopulation))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;

        // Act
        await sut.UpdatePopulationAsync(sousPrefecture.Nom, updatedPopulation, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.SousPrefectures);
        Assert.Equal(sousPrefecture.Id, persisted.Id);
        Assert.Equal(sousPrefecture.Nom, persisted.Nom);
        Assert.Equal(updatedPopulation, persisted.Population);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenNoExistingSousPrefecture_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenNoExistingSousPrefecture_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            }
        };

        // Act
        async Task act() => await sut.UpdatePopulationAsync(sousPrefecture.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenExistingSousPrefecture_ThenUpdateCoordonneesGeographiques()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenExistingSousPrefecture_ThenUpdateCoordonneesGeographiques))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 5.345m,
            Longitude = -4.008m
        };

        // Act
        await sut.UpdateCoordonneesGeographiquesAsync(sousPrefecture.Nom, coordonneesGeographiques, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.SousPrefectures);
        Assert.Equal(sousPrefecture.Id, persisted.Id);
        Assert.Equal(sousPrefecture.Nom, persisted.Nom);
        Assert.Equal(coordonneesGeographiques, persisted.CoordonneesGeographiques);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingSousPrefecture_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingSousPrefecture_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            }
        };
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 5.345m,
            Longitude = -4.008m
        };

        // Act
        async Task act() => await sut.UpdateCoordonneesGeographiquesAsync(sousPrefecture.Nom, coordonneesGeographiques, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingSousPrefecture_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingSousPrefecture_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);
        var sousPrefecture = new SousPrefecture
        {
            Id = Guid.NewGuid(),
            Nom = "Abobo",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan",
                RegionId = Guid.NewGuid(),
                Region = new Region
                {
                    Id = Guid.NewGuid(),
                    Nom = "Lagunes",
                    DistrictId = Guid.NewGuid(),
                    District = new District
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Abidjan"
                    }
                }
            }
        };
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(sousPrefecture.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.SousPrefectures);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingSousPrefecture_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingSousPrefecture_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
