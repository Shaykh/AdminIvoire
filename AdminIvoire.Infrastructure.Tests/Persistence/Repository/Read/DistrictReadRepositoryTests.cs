using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class DistrictReadRepositoryTests
{
    [Fact]
    public async Task GivenGetById_WhenExistingDistrict_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenExistingDistrict_ThenReturnEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Vallée du Bandaman"
        };
        await context.Districts.AddAsync(district);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByIdAsync(district.Id, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(district.Id, result.Id);
        Assert.Equal(district.Nom, result.Nom);
    }

    [Fact]
    public async Task GivenGetById_WhenNotFoundDistrict_ThenThrowDataException()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenNotFoundDistrict_ThenThrowDataException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);

        //Act
        async Task<District> act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenGetAll_WhenExistingDistricts_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAll_WhenExistingDistricts_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);
        var districts = new List<District>
        {
            new() { Id = Guid.NewGuid(), Nom = "Vallée du Bandaman" },
            new() { Id = Guid.NewGuid(), Nom = "Sud-Comoé" },
            new() { Id = Guid.NewGuid(), Nom = "Lagunes" }
        };
        await context.Districts.AddRangeAsync(districts);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Equal(districts.Count, result.Count);
    }

    [Fact]
    public async Task GivenGetAll_WhenNoDistricts_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAll_WhenNoDistricts_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByName_WhenExistingDistrict_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByName_WhenExistingDistrict_ThenReturnEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);
        var district = new District
        {
            Id = Guid.NewGuid(),
            Nom = "Vallée du Bandaman"
        };
        await context.Districts.AddAsync(district);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByNomAsync(district.Nom, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(district.Id, result.Id);
        Assert.Equal(district.Nom, result.Nom);
    }

    [Fact]
    public async Task GivenGetByName_WhenNotFoundDistrict_ThenReturnNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByName_WhenNotFoundDistrict_ThenReturnNull))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);

        //Act
        var result = await sut.GetByNomAsync("Vallée du Bandaman", CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenExistingDistricts_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenExistingDistricts_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);
        var districts = new List<District>
        {
            new() { Id = Guid.NewGuid(), Nom = "Vallée du Bandaman" },
            new() { Id = Guid.NewGuid(), Nom = "Sud-Comoé" },
            new() { Id = Guid.NewGuid(), Nom = "Lagunes" }
        };
        await context.Districts.AddRangeAsync(districts);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(districts.Count, result.Count);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenNoDistrict_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenNoDistrict_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DistrictReadRepository(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

}
