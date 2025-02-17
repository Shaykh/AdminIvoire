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
