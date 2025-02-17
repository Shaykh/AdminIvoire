using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class RegionWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidRegion_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidRegion_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionWriteRepository(context);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Lagunes",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan"
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        await sut.AddAsync(region, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Regions);
        Assert.Equal(region.Id, persisted.Id);
        Assert.Equal(region.Nom, persisted.Nom);
        Assert.Equal(region.DistrictId, persisted.DistrictId);
        Assert.Equal(region.Population, persisted.Population);
        Assert.Equal(region.Superficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingRegion_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingRegion_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionWriteRepository(context);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Lagunes",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan"
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Regions.AddAsync(region);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        region.Population = updatedPopulation;
        region.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(region, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Regions);
        Assert.Equal(region.Id, persisted.Id);
        Assert.Equal(region.Nom, persisted.Nom);
        Assert.Equal(region.DistrictId, persisted.DistrictId);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingRegion_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingRegion_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionWriteRepository(context);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Lagunes",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan"
            },
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateAsync(region, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingRegion_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingRegion_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionWriteRepository(context);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Lagunes",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Abidjan"
            },
            Population = 0,
            Superficie = 0,
        };
        await context.Regions.AddAsync(region);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(region.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Regions);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingRegion_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingRegion_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
