using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Application.ApiClient;

public interface IGeocodingApi
{
    Task<CoordonneesGeographiques> GetCoordonneesGeographiquesAsync(string localite, CancellationToken cancellationToken);
}
