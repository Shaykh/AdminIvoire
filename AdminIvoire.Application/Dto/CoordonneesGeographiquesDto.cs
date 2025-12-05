namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO représentant des coordonnées géographiques
/// </summary>
/// <param name="Latitude">La latitude en degrés décimaux</param>
/// <param name="Longitude">La longitude en degrés décimaux</param>
public record CoordonneesGeographiquesDto(decimal Latitude, decimal Longitude);