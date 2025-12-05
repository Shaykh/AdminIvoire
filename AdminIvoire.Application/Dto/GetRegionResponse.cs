namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO de réponse pour récupérer une région avec ses départements
/// </summary>
/// <param name="Id">L'identifiant unique de la région</param>
/// <param name="Nom">Le nom de la région</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="DistrictId">L'identifiant du district auquel appartient la région</param>
/// <param name="DistrictNom">Le nom du district auquel appartient la région</param>
/// <param name="Departements">La liste des départements appartenant à cette région</param>
public record GetRegionResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid DistrictId,
    string DistrictNom,
    IEnumerable<LocaliteDto> Departements);
