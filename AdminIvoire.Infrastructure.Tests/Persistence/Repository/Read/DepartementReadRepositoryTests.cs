using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class DepartementReadRepositoryTests
{
    [Fact]
    public async Task GivenGetById_WhenExistingDepartement_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenExistingDepartement_ThenReturnEntity))
            .Options;

        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);
        var departement = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Bouaké",
            RegionId = Guid.NewGuid(),
            Region = new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Gbêkê",
                DistrictId = Guid.NewGuid(),
                District = new District
                {
                    Id = Guid.NewGuid(),
                    Nom = "Vallée du Bandaman"
                }
            }
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByIdAsync(departement.Id, CancellationToken.None);

        //Assert
        Assert.Equal(departement.Id, result.Id);
        Assert.Equal(departement.Nom, result.Nom);
        Assert.Equal(departement.RegionId, result.RegionId);
    }

    [Fact]
    public async Task GivenGetById_WhenNotFoundDepartement_ThenThrowDataException()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenNotFoundDepartement_ThenThrowDataException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);

        //Act
        async Task<Departement> act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<DataException>((Func<Task<Departement>>)act);
    }

    [Fact]
    public async Task GivenGetAllByRegionId_WhenExistingDepartements_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByRegionId_WhenExistingDepartements_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var (regionId, departementsOfRegion) = await SetDepartementsForTest(context);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetAllByRegionIdAsync(regionId, CancellationToken.None);

        //Assert
        Assert.Equal(departementsOfRegion.Count, result.Count);
        Assert.Equal(departementsOfRegion[0].Id, result[0].Id);
        Assert.Equal(departementsOfRegion[0].Nom, result[0].Nom);
        Assert.Equal(departementsOfRegion[1].Id, result[1].Id);
        Assert.Equal(departementsOfRegion[1].Nom, result[1].Nom);
        Assert.Equal(departementsOfRegion[2].Id, result[2].Id);
        Assert.Equal(departementsOfRegion[2].Nom, result[2].Nom);
    }

    [Fact]
    public async Task GivenGetAllByRegionId_WhenNotFoundDepartements_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByRegionId_WhenNotFoundDepartements_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetAllByRegionIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetAll_WhenExistingDepartements_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAll_WhenExistingDepartements_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var (_, departementsOfRegion) = await SetDepartementsForTest(context);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Equal(departementsOfRegion.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAll_WhenNotFoundDepartements_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAll_WhenNotFoundDepartements_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByNom_WhenExistingDepartement_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNom_WhenExistingDepartement_ThenReturnEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);
        var departement = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Bouaké",
            RegionId = Guid.NewGuid(),
            Region = new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Gbêkê",
                DistrictId = Guid.NewGuid(),
                District = new District
                {
                    Id = Guid.NewGuid(),
                    Nom = "Vallée du Bandaman"
                }
            }
        };
        await context.Departements.AddAsync(departement);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByNomAsync(departement.Nom, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(departement.Id, result.Id);
        Assert.Equal(departement.Nom, result.Nom);
        Assert.Equal(departement.RegionId, result.RegionId);
    }

    [Fact]
    public async Task GivenGetByNom_WhenNotFoundDepartement_ThenReturnNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNom_WhenNotFoundDepartement_ThenReturnNull))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetByNomAsync("Katiola", CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenExistingDepartements_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenExistingDepartements_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);
        var (_, communes) = await SetDepartementsForTest(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(communes.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenNoDepartement_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenNoDepartement_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new DepartementReadRepository(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    private static async Task<(Guid regionId, List<Departement> departementsOfRegion)> SetDepartementsForTest(LocaliteContext context)
    {
        var regionId = Guid.NewGuid();
        var region = new Region
        {
            Id = regionId,
            Nom = "Gbêkê",
            DistrictId = Guid.NewGuid(),
            District = new District
            {
                Id = Guid.NewGuid(),
                Nom = "Vallée du Bandaman"
            }
        };
        List<Departement> departementsOfRegion = [
            new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Bouaké",
                RegionId = Guid.NewGuid(),
                Region = region
            },new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Beoumi",
                RegionId = Guid.NewGuid(),
                Region = region
            },new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Sakassou",
                RegionId = Guid.NewGuid(),
                Region = region
            }
        ];
        var otherDepartement = new Departement
        {
            Id = Guid.NewGuid(),
            Nom = "Katiola",
            RegionId = Guid.NewGuid(),
            Region = new Region
            {
                Id = Guid.NewGuid(),
                Nom = "Hambol",
                DistrictId = Guid.NewGuid(),
                District = new District
                {
                    Id = Guid.NewGuid(),
                    Nom = "Vallée du Bandaman"
                }
            }
        };
        await context.Departements.AddAsync(otherDepartement);
        await context.Departements.AddRangeAsync(departementsOfRegion);
        await context.SaveChangesAsync();
        return (regionId, departementsOfRegion);
    }
}
