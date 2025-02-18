using AdminIvoire.Domain.Entite;
using AdminIvoire.Infrastructure.Persistence.Repository.Read;
using AdminIvoire.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository.Read;

public class SousPrefectureReadRepositoryTests
{
    [Fact]
    public async Task GivenGetById_WhenExistingSousPrefecture_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenExistingSousPrefecture_ThenReturnEntity))
            .Options;

        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        var sousPrefecture = new SousPrefecture
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
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByIdAsync(sousPrefecture.Id, CancellationToken.None);

        //Assert
        Assert.Equal(sousPrefecture.Id, result.Id);
        Assert.Equal(sousPrefecture.Nom, result.Nom);
        Assert.Equal(sousPrefecture.DepartementId, result.DepartementId);
    }

    [Fact]
    public async Task GivenGetById_WhenNotFoundSousPrefecture_ThenThrowDataException()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetById_WhenNotFoundSousPrefecture_ThenThrowDataException))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);

        //Act
        async Task<SousPrefecture> act() => await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<DataException>((Func<Task<SousPrefecture>>)act);
    }

    [Fact]
    public async Task GivenGetAllByDepartementIdAsync_WhenExistingSousPrefecturesWithDepartementId_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDepartementIdAsync_WhenExistingSousPrefecturesWithDepartementId_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        (Guid departementId, List<SousPrefecture> sousPrefecturesOfDepartement) = await SetSousPrefecturesForTest(context);

        //Act
        var result = await sut.GetAllByDepartementIdAsync(departementId, CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(sousPrefecturesOfDepartement.Count, result.Count);
        Assert.All(result, c => Assert.Equal(departementId, c.DepartementId));
    }

    [Fact]
    public async Task GivenGetAllByDepartementIdAsync_WhenNoSousPrefectureWithDepartementId_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllByDepartementIdAsync_WhenNoSousPrefectureWithDepartementId_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        await SetSousPrefecturesForTest(context);

        //Act
        var result = await sut.GetAllByDepartementIdAsync(Guid.NewGuid(), CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetAlldAsync_WhenExistingSousPrefectures_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAlldAsync_WhenExistingSousPrefectures_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        var (_, sousPrefectures) = await SetSousPrefecturesForTest(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(sousPrefectures.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllAsync_WhenNoSousPrefecture_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllAsync_WhenNoSousPrefecture_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);

        //Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenExistingSousPrefecture_ThenReturnEntity()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenExistingSousPrefecture_ThenReturnEntity))
            .Options;

        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        var sousPrefecture = new SousPrefecture
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
        await context.SousPrefectures.AddAsync(sousPrefecture);
        await context.SaveChangesAsync();

        //Act
        var result = await sut.GetByNomAsync(sousPrefecture.Nom, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(sousPrefecture.Nom, result.Nom);
        Assert.Equal(sousPrefecture.Id, result.Id);
        Assert.Equal(sousPrefecture.DepartementId, result.DepartementId);
    }

    [Fact]
    public async Task GivenGetByNomAsync_WhenNotFoundSousPrefecture_ThenReturnNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetByNomAsync_WhenNotFoundSousPrefecture_ThenReturnNull))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);

        //Act
        var result = await sut.GetByNomAsync("Lobobo", CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenExistingSousPrefectures_ThenReturnEntities()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenExistingSousPrefectures_ThenReturnEntities))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);
        var (_, sousPrefectures) = await SetSousPrefecturesForTest(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(sousPrefectures.Count + 1, result.Count);
    }

    [Fact]
    public async Task GivenGetAllNomsAsync_WhenNoSousPrefecture_ThenReturnEmptyList()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetAllNomsAsync_WhenNoSousPrefecture_ThenReturnEmptyList))
            .Options;
        using var context = new LocaliteContext(options);
        var sut = new SousPrefectureReadRepository(context);

        //Act
        var result = await sut.GetAllNomsAsync(CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }

    private static async Task<(Guid departementId, List<SousPrefecture> sousPrefecturesOfDepartement)> SetSousPrefecturesForTest(LocaliteContext context)
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
        List<SousPrefecture> sousPrefecturesOfDepartement = [
            new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Abobo",
                DepartementId = departementId,
                Departement = departement
            },
            new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Yopougon",
                DepartementId = departementId,
                Departement = departement
            },
            new SousPrefecture
            {
                Id = Guid.NewGuid(),
                Nom = "Treichville",
                DepartementId = departementId,
                Departement = departement
            }
            ];
        var otherSousPrefecture = new SousPrefecture
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
        await context.SousPrefectures.AddAsync(otherSousPrefecture);
        await context.SousPrefectures.AddRangeAsync(sousPrefecturesOfDepartement);
        await context.SaveChangesAsync();
        return (departementId, sousPrefecturesOfDepartement);
    }
}
