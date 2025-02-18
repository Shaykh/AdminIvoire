using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.ValueObject;
using AdminIvoire.Infrastructure.ApiClient;
using AdminIvoire.Infrastructure.ApiClient.Model;
using AdminIvoire.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace AdminIvoire.Infrastructure.Tests.ApiClient;

public class GoogleGeocodingApiClientTests
{
    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenCorrectConfigurationAndValidApiResponse_ThenReturnCoordonneesGeographiques()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        const double expectedLatitude = 5.359951;
        const double expectedLongitude = -4.008256;
        var expectedCoordonneesGeographiques = new CoordonneesGeographiques
        {
            Latitude = Convert.ToDecimal(expectedLatitude),
            Longitude = Convert.ToDecimal(expectedLongitude)
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        var httpResponseContent = new GoogleCoordinateResponse
        {
            Status = "OK",
            Results = [
                new Result
                {
                    AddressComponents = [],
                    FormattedAddress = "Abidjan, Côte d'Ivoire",
                    Geometry = new Geometry()
                    {
                        Location = new Location()
                        {
                            Lat = expectedLatitude,
                            Lng = expectedLongitude
                        },
                        LocationType = "ROOFTOP",
                        Viewport = new Viewport
                        {
                            Northeast = new Location
                            {
                                Lat = expectedLatitude,
                                Lng = expectedLongitude
                            },
                            Southwest = new Location
                            {
                                Lat = expectedLatitude,
                                Lng = expectedLongitude
                            }
                        },
                        Bounds = new Bound
                        {
                            Northeast = new Location
                            {
                                Lat = expectedLatitude,
                                Lng = expectedLongitude
                            },
                            Southwest = new Location
                            {
                                Lat = expectedLatitude,
                                Lng = expectedLongitude
                            }
                        }
                    },
                    Types = [],
                    PlaceId = "fake-place"
                }
            ]
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(httpResponseContent))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        var result = await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCoordonneesGeographiques, result);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenNoBaseUrlConfiguration_ThrowException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, new HttpClient(), loggerMock.Object);
        var localite = "Abidjan";

        //Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ConfigurationException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenNoApiKeyConfiguration_ThrowException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", "baseUrl")
            ])
            .Build();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, new HttpClient(), loggerMock.Object);
        var localite = "Abidjan";

        //Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ConfigurationException>(act);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseIsNotSuccess_ThenThrowException(HttpStatusCode apiResponseStatusCode)
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = apiResponseStatusCode,
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseIsNotWellFormated_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject("BadHttpResponseContent"))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseContentNull_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = null
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseContentStatusNotOK_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        var httpResponseContent = new GoogleCoordinateResponse
        {
            Status = "KO",
            Results = [
                new Result
                {
                    AddressComponents = [],
                    FormattedAddress = "Abidjan, Côte d'Ivoire",
                    Geometry = new Geometry()
                    {
                        Location = new Location()
                        {
                            Lat = 0,
                            Lng = 0
                        },
                        LocationType = "ROOFTOP",
                        Viewport = new Viewport
                        {
                            Northeast = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            },
                            Southwest = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            }
                        },
                        Bounds = new Bound
                        {
                            Northeast = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            },
                            Southwest = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            }
                        }
                    },
                    Types = [],
                    PlaceId = "fake-place"
                }
            ]
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(httpResponseContent))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseResultIsEmpty_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        var httpResponseContent = new GoogleCoordinateResponse
        {
            Status = "OK",
            Results = []
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(httpResponseContent))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseResultWithNoGeometry_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        var httpResponseContent = new GoogleCoordinateResponse
        {
            Status = "OK",
            Results = [
                new Result
                {
                    AddressComponents = [],
                    FormattedAddress = "Abidjan, Côte d'Ivoire",
                    Geometry = null,
                    Types = [],
                    PlaceId = "fake-place"
                }
            ]
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(httpResponseContent))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    [Fact]
    public async Task GivenGetCoordonneesGeographiquesAsync_WhenApiResponseResultWithNoLocation_ThenThrowException()
    {
        // Arrange
        const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new ("GoogleMaps:BaseUrl", GeocodingApiUrl),
                new ("GoogleMaps:ApiKey", "fake-api-key")
            ])
            .Build();
        var httpClient = CreateHttpClientForTest(out var handlerMock, GeocodingApiUrl);
        var httpResponseContent = new GoogleCoordinateResponse
        {
            Status = "OK",
            Results = [
                new Result
                {
                    AddressComponents = [],
                    FormattedAddress = "Abidjan, Côte d'Ivoire",
                    Geometry = new Geometry()
                    {
                        Location = null,
                        LocationType = "ROOFTOP",
                        Viewport = new Viewport
                        {
                            Northeast = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            },
                            Southwest = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            }
                        },
                        Bounds = new Bound
                        {
                            Northeast = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            },
                            Southwest = new Location
                            {
                                Lat = 0,
                                Lng = 0
                            }
                        }
                    },
                    Types = [],
                    PlaceId = "fake-place"
                }
            ]
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(httpResponseContent))
            }).Verifiable();
        var loggerMock = new Mock<ILogger<GoogleGeocodingApiClient>>();
        var sut = new GoogleGeocodingApiClient(configuration, httpClient, loggerMock.Object);
        var localite = "Abidjan";

        // Act
        async Task<CoordonneesGeographiques> act() => await sut.GetCoordonneesGeographiquesAsync(localite, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<ApiCallException>(act);
    }

    private static HttpClient CreateHttpClientForTest(out Mock<HttpMessageHandler> handlerMock, string baseAddress)
    {
        handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(baseAddress)
        };
    }
}
