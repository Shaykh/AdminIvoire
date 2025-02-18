using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class RegionReadRepositoryTests
{
    [Fact]
    public async Task GivenGetAllByDistrictIdAsync_WhenExistingRegions_ThenReturnEntityies()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDistrictIdAsync_WhenExistingRegions_ThenReturnEntityies))
            .Options;

        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        (var districtId, var regionsOfDistrict) = await SetRegionsForTest(dbContext);

        // Act
        var regions = await sut.GetAllByDistrictIdAsync(districtId, CancellationToken.None);

        // Assert
        Assert.NotEmpty(regions);
        Assert.Equal(regionsOfDistrict.Count, regions.Count);
    }

    [Fact]
    public async Task GivenGetAllByDistrictIdAsync_WhenNotExistingRegion_ThenReturnEmptyList()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDistrictIdAsync_WhenNotExistingRegion_ThenReturnEmptyList))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        await SetRegionsForTest(dbContext);

        // Act
        var regions = await sut.GetAllByDistrictIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.Empty(regions);
    }

    [Fact]
    public async Task GivenGetByIdAsync_WhenExistingRegion_ThenReturnEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByIdAsync_WhenExistingRegion_ThenReturnEntity))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Gbêkê",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Vallée du Bandaman"
            }
        };
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await sut.GetByIdAsync(region.Id, CancellationToken.None);

        // Assert
        Assert.Equal(region.Id, result.Id);
        Assert.Equal(region.Nom, result.Nom);
        Assert.Equal(region.DistrictId, result.DistrictId);
    }

    [Fact]
    public async Task GivenGetByIdAsync_WithNotExistingRegion_ThenThrowDataException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByIdAsync_WithNotExistingRegion_ThenThrowDataException))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        await SetRegionsForTest(dbContext);

        // Act 
        async Task<Region> act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DataException>((Func<Task<Region>>)act);
    }

    [Fact]
    public async Task GivenGetAllAsync_WhenExistingRegions_ThenReturnEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenExistingRegions_ThenReturnEntities))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        (_, var regionsOfDistrict) = await SetRegionsForTest(dbContext);

        // Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(regionsOfDistrict.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllAsync_WhenNotExistingRegions_ThenReturnEmptyList()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenNotExistingRegions_ThenReturnEmptyList))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);

        // Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByNameAsync_WhenExistingRegion_ThenReturnEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNameAsync_WhenExistingRegion_ThenReturnEntity))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        var region = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "Gbêkê",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Vallée du Bandaman"
            }
        };
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await sut.GetByNomAsync(region.Nom, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(region.Id, result.Id);
        Assert.Equal(region.Nom, result.Nom);
        Assert.Equal(region.DistrictId, result.DistrictId);
    }

    [Fact]
    public async Task GivenGetByNameAsync_WhenNotFoundRegion_ThenReturnNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNameAsync_WhenNotFoundRegion_ThenReturnNull))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var sut = new RegionReadRepository(dbContext);
        
        // Act
        var result = await sut.GetByNomAsync("Gbêkê", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenExistingRegions_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenExistingRegions_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionReadRepository(context);
        var (_, regions) = await SetRegionsForTest(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(regions.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenNoRegion_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenNoRegion_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new RegionReadRepository(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    private static async Task<(Guid districtId, List<Region> regionsOfDistrict)> SetRegionsForTest(LocaliteContext context)
    {
        var districtId = Guid.NewGuid();
        var district = new District
        {
            Id = districtId,
            Nom = "Vallée du Bandaman"
        };
        List<Region> regionsOfDistrict = [
            new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Gbêkê",
                DistrictId = districtId,
                District = district
            },new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Hambol",
                DistrictId = districtId,
                District = district
            },
        ];
        var otherRegion = new Region
        {
            Id = Guid.NewGuid(),
            Nom = "BAGOUE",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "SAVANES"
            }
        };
        await context.Regions.AddAsync(otherRegion);
        await context.Regions.AddRangeAsync(regionsOfDistrict);
        await context.SaveChangesAsync();
        return (districtId, regionsOfDistrict);
    }
}
