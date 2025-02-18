using AdminIvoire.Application.ApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Infrastructure.ApiClient;

public static class ServicesExtensions
{
    public static IServiceCollection AddGeocodingApiClient(this IServiceCollection services)
    {
        services.AddHttpClient<IGeocodingApiClient, GoogleGeocodingApiClient>();

        return services;
    }
}
