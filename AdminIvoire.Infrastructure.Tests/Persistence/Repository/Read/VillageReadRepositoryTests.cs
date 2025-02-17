using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class VillageReadRepositoryTests
{
    [Fact]
    public async Task GivenGetAllAsync_WhenExistingVillages_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenExistingVillages_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);
        var (_, villages) = await SetVillagesForTest(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(villages.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllAsync_WhenNoExistingVillages_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenNoExistingVillages_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetAllBySousPrefectureIdAsync_WhenExistingVillages_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllBySousPrefectureIdAsync_WhenExistingVillages_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);
        var (sousPrefectureId, villages) = await SetVillagesForTest(context);

        //Act
        var result = await sut.GetAllBySousPrefectureIdAsync(sousPrefectureId, CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(villages.Count, result.Count);
    }

    [Fact]
    public async Task GivenGetAllBySousPrefectureIdAsync_WhenNoExistingVillages_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllBySousPrefectureIdAsync_WhenNoExistingVillages_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);

        //Act
        var result = await sut.GetAllBySousPrefectureIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByIdAsync_WhenExistingVillage_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByIdAsync_WhenExistingVillage_ThenReturnEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Keledjian",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Samatiguila",
                DepartementId = Guid.NewGuid(),
                Departement = new Departement
                {
                    Id = Guid.NewGuid(),
                    Nom = "Samatiguila",
                    RegionId = Guid.NewGuid(),
                    Region = new Region
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Kabadougou",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Denguele"
                        }
                    }
                }
            }
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByIdAsync(village.Id, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(village.Id, result.Id);
        Assert.Equal(village.Nom, result.Nom);
        Assert.Equal(village.SousPrefectureId, result.SousPrefectureId);
    }

    [Fact]
    public async Task GivenGetByIdAsync_WhenNonExistingVillage_ThenThrowDataException()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByIdAsync_WhenNonExistingVillage_ThenThrowDataException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);

        //Act
        async Task act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<DataException>(act);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenExistingVillage_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenExistingVillage_ThenReturnEntity))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);
        var village = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Keledjian",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Samatiguila",
                DepartementId = Guid.NewGuid(),
                Departement = new Departement
                {
                    Id = Guid.NewGuid(),
                    Nom = "Samatiguila",
                    RegionId = Guid.NewGuid(),
                    Region = new Region
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Kabadougou",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Denguele"
                        }
                    }
                }
            }
        };
        await context.Villages.AddAsync(village);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByNomAsync(village.Nom, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(village.Id, result.Id);
        Assert.Equal(village.Nom, result.Nom);
        Assert.Equal(village.SousPrefectureId, result.SousPrefectureId);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenNoExistingVillage_ThenReturnNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenNoExistingVillage_ThenReturnNull))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new VillageReadRepository(context);

        //Act
        var result = await sut.GetByNomAsync("Keledjian", CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    private static async Task<(Guid sousPrefectureId, List<Village> villagesOfSousPrefecture)> SetVillagesForTest(LocaliteContext context)
    {
        var sousPrefectureId = Guid.NewGuid();
        var sousPrefecture = new SousPrefecture
        {
            Id = sousPrefectureId,
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
                        Nom = "Lagunes"
                    }
                }
            }
        };
        List<Village> villagesOfSousPrefecture = [
            new Village
            {
                Id = Guid.NewGuid(),
                Nom = "Akeikoi",
                SousPrefectureId = sousPrefectureId,
                SousPrefecture = sousPrefecture
            },
            new Village
            {
                Id = Guid.NewGuid(),
                Nom = "Anonkoi",
                SousPrefectureId = sousPrefectureId,
                SousPrefecture = sousPrefecture
            },
            new Village
            {
                Id = Guid.NewGuid(),
                Nom = "Agnissankoi",
                SousPrefectureId = sousPrefectureId,
                SousPrefecture = sousPrefecture
            }
            ];
        var otherVillage = new Village
        {
            Id = Guid.NewGuid(),
            Nom = "Keledjian",
            SousPrefectureId = Guid.NewGuid(),
            SousPrefecture = new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Samatiguila",
                DepartementId = Guid.NewGuid(),
                Departement = new Departement
                {
                    Id = Guid.NewGuid(),
                    Nom = "Samatiguila",
                    RegionId = Guid.NewGuid(),
                    Region = new Region
                    {
                        Id = Guid.NewGuid(),
                        Nom = "Kabadougou",
                        DistrictId = Guid.NewGuid(),
                        District = new District
                        {
                            Id = Guid.NewGuid(),
                            Nom = "Denguele"
                        }
                    }
                }
            }
        };
        await context.Villages.AddAsync(otherVillage);
        await context.Villages.AddRangeAsync(villagesOfSousPrefecture);
        await context.SaveChangesAsync();
        return (sousPrefectureId, villagesOfSousPrefecture);
    }
}
