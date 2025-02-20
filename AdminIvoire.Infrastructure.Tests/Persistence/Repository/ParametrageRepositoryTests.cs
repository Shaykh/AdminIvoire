using AdminIvoire.Application.Parametrage;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository;

public class ParametrageRepositoryTests
{
    [Fact]
    public async Task GivenGetParametrageAsync_WhenParametrageExist_ThenReturnEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetParametrageAsync_WhenParametrageExist_ThenReturnEntity))
            .Options;

        using var dbContext = new LocaliteContext(options);
        var loggerMock = new Mock<ILogger<ParametrageRepository>>();
        var parametrageRepository = new ParametrageRepository(loggerMock.Object, dbContext);
        var parametrage = new ParametrageEntity { Key = "Key", Value = "Value" };
        await dbContext.Set<ParametrageEntity>().AddAsync(parametrage);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await parametrageRepository.GetParametrageAsync("Key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Key", result.Key);
        Assert.Equal("Value", result.Value);
    }

    [Fact]
    public async Task GivenGetParametrageAsync_WhenParametrageDoesNotExist_ThenReturnNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenGetParametrageAsync_WhenParametrageDoesNotExist_ThenReturnNull))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var loggerMock = new Mock<ILogger<ParametrageRepository>>();
        var parametrageRepository = new ParametrageRepository(loggerMock.Object, dbContext);

        // Act
        var result = await parametrageRepository.GetParametrageAsync("UnknownKey");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenSetParametrageAsyn_WhenParametrage_ThenAddEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenSetParametrageAsyn_WhenParametrage_ThenAddEntity))
            .Options;
        using var dbContext = new LocaliteContext(options);
        var loggerMock = new Mock<ILogger<ParametrageRepository>>();
        var parametrageRepository = new ParametrageRepository(loggerMock.Object, dbContext);
        var parametrage = new ParametrageEntity { Key = "Key", Value = "Value" };

        // Act
        await parametrageRepository.SetParametrageAsync(parametrage);

        // Assert
        var result = await dbContext.Set<ParametrageEntity>().FirstOrDefaultAsync();
        Assert.NotNull(result);
        Assert.Equal(parametrage.Key, result.Key);
        Assert.Equal(parametrage.Value, result.Value);
    }
}
