using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class CommuneReadRepositoryTests
{
    [Fact]
    public async Task GivenGetById_WhenExistingCommune_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenExistingCommune_ThenReturnEntity))
            .Options;

        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);
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

        //Act
        var result = await sut.GetByIdAsync(commune.Id, CancellationToken.None);

        //Assert
        Assert.Equal(commune.Id, result.Id);
        Assert.Equal(commune.Nom, result.Nom);
        Assert.Equal(commune.DepartementId, result.DepartementId);
    }

    [Fact]
    public async Task GivenGetById_WhenNotFoundCommune_ThenThrowDataException()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenNotFoundCommune_ThenThrowDataException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);

        //Act
        async Task<Commune> act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<DataException>((Func<Task<Commune>>)act);
    }

    [Fact]
    public async Task GivenGetAllByDepartementIdAsync_WhenExistingCommunesWithDepartementId_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDepartementIdAsync_WhenExistingCommunesWithDepartementId_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);
        (Guid departementId, List<Commune> communesOfDepartement) = await SetCommunesForTest(context);

        //Act
        var result = await sut.GetAllByDepartementIdAsync(departementId, CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(communesOfDepartement.Count, result.Count);
        Assert.All(result, c => Assert.Equal(departementId, c.DepartementId));
    }

    [Fact]
    public async Task GivenGetAllByDepartementIdAsync_WhenNoCommuneWithDepartementId_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDepartementIdAsync_WhenNoCommuneWithDepartementId_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);
        await SetCommunesForTest(context);

        //Act
        var result = await sut.GetAllByDepartementIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetAlldAsync_WhenExistingCommunes_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAlldAsync_WhenExistingCommunes_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);
        var (_, communes) = await SetCommunesForTest(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(communes.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllAsync_WhenNoCommune_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenNoCommune_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenExistingCommune_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenExistingCommune_ThenReturnEntity))
            .Options;

        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);
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

        //Act
        var result = await sut.GetByNomAsync(commune.Nom, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(commune.Nom, result.Nom);
        Assert.Equal(commune.Id, result.Id);
        Assert.Equal(commune.DepartementId, result.DepartementId);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenNotFoundCommune_ThenReturnNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenNotFoundCommune_ThenReturnNull))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new CommuneReadRepository(context);

        //Act
        var result = await sut.GetByNomAsync("Lobobo", CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    private static async Task<(Guid departementId, List<Commune> communesOfDepartement)> SetCommunesForTest(LocaliteContext context)
    {
        var departementId = Guid.NewGuid();
        var departement = new Departement
        {
            Id = departementId,
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
        };
        List<Commune> communesOfDepartement = [
            new Commune
            {
                Id = Guid.NewGuid(),
                Nom = "Abobo",
                DepartementId = departementId,
                Departement = departement
            },
            new Commune
            {
                Id = Guid.NewGuid(),
                Nom = "Yopougon",
                DepartementId = departementId,
                Departement = departement
            },
            new Commune
            {
                Id = Guid.NewGuid(),
                Nom = "Treichville",
                DepartementId = departementId,
                Departement = departement
            }
            ];
        var otherCommune = new Commune
        {
            Id = Guid.NewGuid(),
            Nom = "Bouake",
            DepartementId = Guid.NewGuid(),
            Departement = new Departement
            {
                Id = Guid.NewGuid(),
                Nom = "Bouake",
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
            }
        };
        await context.Communes.AddAsync(otherCommune);
        await context.Communes.AddRangeAsync(communesOfDepartement);
        await context.SaveChangesAsync();
        return (departementId, communesOfDepartement);
    }
}
