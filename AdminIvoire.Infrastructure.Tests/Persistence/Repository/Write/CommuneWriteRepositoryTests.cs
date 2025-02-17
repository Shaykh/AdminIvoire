using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class CommuneWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidCommune_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidCommune_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneWriteRepository(context);
        var commune = new Commune
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
        await sut.AddAsync(commune, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Communes);
        Assert.Equal(commune.Id, persisted.Id);
        Assert.Equal(commune.Nom, persisted.Nom);
        Assert.Equal(commune.DepartementId, persisted.DepartementId);
        Assert.Equal(commune.Population, persisted.Population);
        Assert.Equal(commune.Superficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingCommune_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingCommune_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneWriteRepository(context);
        var commune = new Commune
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
        await context.Communes.AddAsync(commune);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        commune.Population = updatedPopulation;
        commune.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(commune, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Communes);
        Assert.Equal(commune.Id, persisted.Id);
        Assert.Equal(commune.Nom, persisted.Nom);
        Assert.Equal(commune.DepartementId, persisted.DepartementId);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingCommune_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingCommune_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneWriteRepository(context);
        var commune = new Commune
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
        async Task act() => await sut.UpdateAsync(commune, CancellationToken.None); 

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingCommune_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingCommune_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneWriteRepository(context);
        var commune = new Commune
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
        await context.Communes.AddAsync(commune);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(commune.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Communes);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingCommune_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingCommune_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
