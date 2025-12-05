using AdminIvoire.Application.ApiClient;
using AdminIvoire.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminIvoire.Application.Tests.Services;

public class VillageWebSourceServiceTests
{
    private static VillageWebSourceService MakeSut(out Mock<IOpenStreetMapApiClient> openStreetMapApiClientMock)
    {
        openStreetMapApiClientMock = new Mock<IOpenStreetMapApiClient>();
        return new VillageWebSourceService(
            openStreetMapApiClientMock.Object,
            Mock.Of<ILogger<VillageWebSourceService>>());
    }

    private static VillageWebSourceService MakeSut()
    {
        return MakeSut(out _);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalled_ThenReturnVillagesFromApiClient()
    {
        // Arrange
        var sut = MakeSut(out var openStreetMapApiClientMock);
        var sousPrefectureNom = "Test Sous-Prefecture";
        var expectedVillages = new List<string> { "Village1", "Village2" };

        openStreetMapApiClientMock.Setup(x => x.GetVillagesAsync(
                sousPrefectureNom,
                null,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedVillages);

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedVillages.Count, result.Count);
        Assert.Equal(expectedVillages, result);
        openStreetMapApiClientMock.Verify(x => x.GetVillagesAsync(
            sousPrefectureNom,
            null,
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalledWithDepartementNom_ThenReturnVillagesFromApiClient()
    {
        // Arrange
        var sut = MakeSut(out var openStreetMapApiClientMock);
        var sousPrefectureNom = "Test Sous-Prefecture";
        var departementNom = "Test Departement";
        var expectedVillages = new List<string> { "Village1" };

        openStreetMapApiClientMock.Setup(x => x.GetVillagesAsync(
                sousPrefectureNom,
                departementNom,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedVillages);

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, departementNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedVillages.Count, result.Count);
        openStreetMapApiClientMock.Verify(x => x.GetVillagesAsync(
            sousPrefectureNom,
            departementNom,
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenCalledWithAllParameters_ThenReturnVillagesFromApiClient()
    {
        // Arrange
        var sut = MakeSut(out var openStreetMapApiClientMock);
        var sousPrefectureNom = "Test Sous-Prefecture";
        var departementNom = "Test Departement";
        var regionNom = "Test Region";
        var expectedVillages = new List<string> { "Village1", "Village2", "Village3" };

        openStreetMapApiClientMock.Setup(x => x.GetVillagesAsync(
                sousPrefectureNom,
                departementNom,
                regionNom,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedVillages);

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, departementNom, regionNom, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedVillages.Count, result.Count);
        openStreetMapApiClientMock.Verify(x => x.GetVillagesAsync(
            sousPrefectureNom,
            departementNom,
            regionNom,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenApiClientThrowsException_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut(out var openStreetMapApiClientMock);
        var sousPrefectureNom = "Test Sous-Prefecture";

        openStreetMapApiClientMock.Setup(x => x.GetVillagesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenGetVillagesAsync_WhenApiClientReturnsEmptyList_ThenReturnEmptyList()
    {
        // Arrange
        var sut = MakeSut(out var openStreetMapApiClientMock);
        var sousPrefectureNom = "Test Sous-Prefecture";

        openStreetMapApiClientMock.Setup(x => x.GetVillagesAsync(
                sousPrefectureNom,
                null,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await sut.GetVillagesAsync(sousPrefectureNom, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

