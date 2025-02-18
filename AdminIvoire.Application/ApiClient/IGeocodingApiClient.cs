using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Application.ApiClient;

public interface IGeocodingApiClient
{
    Task<CoordonneesGeographiques> GetCoordonneesGeographiquesAsync(string localite, CancellationToken cancellationToken);
}
