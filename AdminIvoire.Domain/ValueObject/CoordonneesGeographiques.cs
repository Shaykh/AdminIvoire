namespace AdminIvoire.Domain.ValueObject;

/// <summary>
/// Value object représentant des coordonnées géographiques (latitude, longitude)
/// </summary>
public record CoordonneesGeographiques
{
    /// <summary>
    /// La latitude en degrés décimaux
    /// </summary>
    public decimal Latitude { get; set; }
    /// <summary>
    /// La longitude en degrés décimaux
    /// </summary>
    public decimal Longitude { get; set; }
}