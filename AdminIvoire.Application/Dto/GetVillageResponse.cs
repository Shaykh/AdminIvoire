namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO de réponse pour récupérer un village
/// </summary>
/// <param name="Id">L'identifiant unique du village</param>
/// <param name="Nom">Le nom du village</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="SousPrefectureId">L'identifiant de la sous-préfecture à laquelle appartient le village</param>
/// <param name="SousPrefectureNom">Le nom de la sous-préfecture à laquelle appartient le village</param>
/// <param name="DepartementNom">Le nom du département auquel appartient le village</param>
/// <param name="RegionNom">Le nom de la région à laquelle appartient le village</param>
/// <param name="DistrictNom">Le nom du district auquel appartient le village</param>
/// <param name="CoordonneesGeographiques">Les coordonnées géographiques du village</param>
public record GetVillageResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid SousPrefectureId,
    string SousPrefectureNom,
    string DepartementNom,
    string RegionNom,
    string DistrictNom,
    CoordonneesGeographiquesDto CoordonneesGeographiques);