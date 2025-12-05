namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO représentant une localité avec ses informations de base
/// </summary>
/// <param name="Id">L'identifiant unique de la localité</param>
/// <param name="Nom">Le nom de la localité</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="CoordonneesGeographiques">Les coordonnées géographiques (latitude, longitude) de la localité, null si non disponibles</param>
public record LocaliteDto(Guid Id, 
    string Nom, 
    decimal Superficie, 
    int Population, 
    CoordonneesGeographiquesDto? CoordonneesGeographiques);
