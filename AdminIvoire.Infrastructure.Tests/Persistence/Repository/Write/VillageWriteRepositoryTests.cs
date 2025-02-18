using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class VillageWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidVillage_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidVillage_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            }
        };

        // Act
        await sut.AddAsync(village, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Villages);
        Assert.Equal(village.Id, persisted.Id);
        Assert.Equal(village.Nom, persisted.Nom);
        Assert.Equal(village.SousPrefectureId, persisted.SousPrefectureId);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingVillage_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingVillage_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        village.Population = updatedPopulation;
        village.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(village, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Villages);
        Assert.Equal(village.Id, persisted.Id);
        Assert.Equal(village.Nom, persisted.Nom);
        Assert.Equal(village.SousPrefectureId, persisted.SousPrefectureId);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingVillage_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingVillage_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateAsync(village, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenExistingVillage_ThenUpdateSuperficie()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenExistingVillage_ThenUpdateSuperficie))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();
        const decimal updatedSuperficie = 354m;

        // Act
        await sut.UpdateSuperficieAsync(village.Nom, updatedSuperficie, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Villages);
        Assert.Equal(village.Id, persisted.Id);
        Assert.Equal(village.Nom, persisted.Nom);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenNoExistingVillage_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenNoExistingVillage_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateSuperficieAsync(village.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenExistingVillage_ThenUpdatePopulation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenExistingVillage_ThenUpdatePopulation))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;

        // Act
        await sut.UpdatePopulationAsync(village.Nom, updatedPopulation, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Villages);
        Assert.Equal(village.Id, persisted.Id);
        Assert.Equal(village.Nom, persisted.Nom);
        Assert.Equal(updatedPopulation, persisted.Population);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenNoExistingVillage_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenNoExistingVillage_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdatePopulationAsync(village.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenExistingVillage_ThenUpdateCoordonneesGeographiques()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenExistingVillage_ThenUpdateCoordonneesGeographiques))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 5.345m,
            Longitude = -4.008m
        };

        // Act
        await sut.UpdateCoordonneesGeographiquesAsync(village.Nom, coordonneesGeographiques, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Villages);
        Assert.Equal(village.Id, persisted.Id);
        Assert.Equal(village.Nom, persisted.Nom);
        Assert.Equal(coordonneesGeographiques, persisted.CoordonneesGeographiques);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingVillage_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingVillage_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 5.345m,
            Longitude = -4.008m
        };

        // Act
        async Task act() => await sut.UpdateCoordonneesGeographiquesAsync(village.Nom, coordonneesGeographiques, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingVillage_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingVillage_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Akeikoi",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
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
                        Nom = "Abidjan",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Abidjan"
                        }
                    }
                }
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(village.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Villages);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingVillage_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingVillage_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
