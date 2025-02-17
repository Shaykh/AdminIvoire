using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Write;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Write;

public class DistrictWriteRepositoryTests
{
    [Fact]
    public async Task GivenAddAsync_WhenValidDistrict_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenAddAsync_WhenValidDistrict_ThenAddEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictWriteRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Abidjan",
            Population = 0,
            Superficie = 0,
        };

        // Act
        await sut.AddAsync(district, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Districts);
        Assert.Equal(district.Id, persisted.Id);
        Assert.Equal(district.Nom, persisted.Nom);
        Assert.Equal(district.Population, persisted.Population);
        Assert.Equal(district.Superficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenExistingDistrict_ThenUpdateEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenExistingDistrict_ThenUpdateEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictWriteRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Abidjan",
            Population = 0,
            Superficie = 0,
        };
        await context.Districts.AddAsync(district);
        await context.SaveChangesAsync();
        const int updatedPopulation = 100000;
        const decimal updatedSuperficie = 354m;
        district.Population = updatedPopulation;
        district.Superficie = updatedSuperficie;

        // Act
        await sut.UpdateAsync(district, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var persisted = Assert.Single(context.Districts);
        Assert.Equal(district.Id, persisted.Id);
        Assert.Equal(district.Nom, persisted.Nom);
        Assert.Equal(updatedPopulation, persisted.Population);
        Assert.Equal(updatedSuperficie, persisted.Superficie);
    }

    [Fact]
    public async Task GivenUpdateAsync_WhenNoExistingDistrict_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenUpdateAsync_WhenNoExistingDistrict_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictWriteRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Abidjan",
            Population = 0,
            Superficie = 0,
        };

        // Act
        async Task act() => await sut.UpdateAsync(district, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenExistingDistrict_ThenRemoveEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenExistingDistrict_ThenRemoveEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictWriteRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Abidjan",
            Population = 0,
            Superficie = 0,
        };
        await context.Districts.AddAsync(district);
        await context.SaveChangesAsync();

        // Act
        await sut.RemoveAsync(district.Id, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Districts);
    }

    [Fact]
    public async Task GivenRemoveAsync_WhenNoExistingDistrict_ThenThrowException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenRemoveAsync_WhenNoExistingDistrict_ThenThrowException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictWriteRepository(context);

        // Act
        async Task act() => await sut.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>(act);
    }
}
