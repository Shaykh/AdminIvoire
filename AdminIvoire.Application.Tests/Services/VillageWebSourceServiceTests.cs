using AdminIvoire.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Services;

public class VillageWebSourceServiceTests
{
    private static VillageWebSourceService MakeSut()
    {
        return new VillageWebSourceService(Mock.Of<ILogger<VillageWebSourceService>>());
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalled_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut();
        var sousPrefectureNom = "Test Sous-Prefecture";

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalledWithDepartementNom_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut();
        var sousPrefectureNom = "Test Sous-Prefecture";
        var departementNom = "Test Departement";

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, departementNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalledWithAllParameters_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut();
        var sousPrefectureNom = "Test Sous-Prefecture";
        var departementNom = "Test Departement";
        var regionNom = "Test Region";

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, departementNom, regionNom, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCancellationTokenIsCancelled_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut();
        var sousPrefectureNom = "Test Sous-Prefecture";
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, cancellationToken: cancellationTokenSource.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

