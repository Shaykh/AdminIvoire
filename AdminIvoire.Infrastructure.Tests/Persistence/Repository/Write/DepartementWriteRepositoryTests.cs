﻿using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class DepartementWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidDepartement_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidDepartement_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        await sut.AddAsync(departement, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Departements);
        Assert.Equal(departement.Id, persisted.Id);
        Assert.Equal(departement.Nom, persisted.Nom);
        Assert.Equal(departement.RegionId, persisted.RegionId);
        Assert.Equal(departement.Population, persisted.Population);
        Assert.Equal(departement.Superficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingDepartement_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingDepartement_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        departement.Population = updatedPopulation;
        departement.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(departement, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Departements);
        Assert.Equal(departement.Id, persisted.Id);
        Assert.Equal(departement.Nom, persisted.Nom);
        Assert.Equal(departement.RegionId, persisted.RegionId);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingDepartement_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingDepartement_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateAsync(departement, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenExistingDepartement_ThenUpdateSuperficie()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenExistingDepartement_ThenUpdateSuperficie))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();
        const decimal updatedSuperficie = 354m;

        // Act
        await sut.UpdateSuperficieAsync(departement.Nom, updatedSuperficie, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Departements);
        Assert.Equal(departement.Id, persisted.Id);
        Assert.Equal(departement.Nom, persisted.Nom);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateSuperficieAsync_WhenNoExistingDepartement_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateSuperficieAsync_WhenNoExistingDepartement_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateSuperficieAsync(departement.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenExistingDepartement_ThenUpdatePopulation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenExistingDepartement_ThenUpdatePopulation))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;

        // Act
        await sut.UpdatePopulationAsync(departement.Nom, updatedPopulation, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Departements);
        Assert.Equal(departement.Id, persisted.Id);
        Assert.Equal(departement.Nom, persisted.Nom);
        Assert.Equal(updatedPopulation, persisted.Population);
    }

    [Fact]
    public async Task GivenUpdatePopulationAsync_WhenNoExistingDepartement_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdatePopulationAsync_WhenNoExistingDepartement_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdatePopulationAsync(departement.Nom, 0, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenExistingDepartement_ThenUpdateCoordonneesGeographiques()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenExistingDepartement_ThenUpdateCoordonneesGeographiques))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();
        var coordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = 5.345m,
            Longitude = -4.008m
        };

        // Act
        await sut.UpdateCoordonneesGeographiquesAsync(departement.Nom, coordonneesGeographiques, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Departements);
        Assert.Equal(departement.Id, persisted.Id);
        Assert.Equal(departement.Nom, persisted.Nom);
        Assert.Equal(coordonneesGeographiques, persisted.CoordonneesGeographiques);
    }

    [Fact]
    public async Task GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingDepartement_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateCoordonneesGeographiquesAsync_WhenNoExistingDepartement_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
        async Task act() => await sut.UpdateCoordonneesGeographiquesAsync(departement.Nom, coordonneesGeographiques, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingDepartement_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingDepartement_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);
        var departement = new Departement
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
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(departement.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Departements);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingDepartement_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingDepartement_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
