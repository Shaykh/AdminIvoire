namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO de réponse pour récupérer un département avec ses sous-préfectures
/// </summary>
/// <param name="Id">L'identifiant unique du département</param>
/// <param name="Nom">Le nom du département</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="RegionId">L'identifiant de la région à laquelle appartient le département</param>
/// <param name="RegionNom">Le nom de la région à laquelle appartient le département</param>
/// <param name="DistrictNom">Le nom du district auquel appartient le département</param>
/// <param name="SousPrefectures">La liste des sous-préfectures appartenant à ce département</param>
/// <param name="CoordonneesGeographiques">Les coordonnées géographiques du département</param>
public record GetDepartementResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid RegionId,
    string RegionNom,
    string DistrictNom,
    IEnumerable<LocaliteDto> SousPrefectures,
    CoordonneesGeographiquesDto CoordonneesGeographiques);
