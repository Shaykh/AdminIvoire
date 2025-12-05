namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO de réponse pour récupérer une sous-préfecture avec ses villages
/// </summary>
/// <param name="Id">L'identifiant unique de la sous-préfecture</param>
/// <param name="Nom">Le nom de la sous-préfecture</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="DepartementId">L'identifiant du département auquel appartient la sous-préfecture</param>
/// <param name="DepartementNom">Le nom du département auquel appartient la sous-préfecture</param>
/// <param name="RegionNom">Le nom de la région à laquelle appartient la sous-préfecture</param>
/// <param name="DistrictNom">Le nom du district auquel appartient la sous-préfecture</param>
/// <param name="Villages">La liste des villages appartenant à cette sous-préfecture</param>
/// <param name="CoordonneesGeographiques">Les coordonnées géographiques de la sous-préfecture</param>
public record GetSousPrefectureResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid DepartementId,
    string DepartementNom,
    string RegionNom,
    string DistrictNom,
    IEnumerable<LocaliteDto> Villages,
    CoordonneesGeographiquesDto CoordonneesGeographiques);
