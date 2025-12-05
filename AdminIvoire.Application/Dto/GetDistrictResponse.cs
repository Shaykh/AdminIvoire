namespace AdminIvoire.Application.Dto;

/// <summary>
/// DTO de réponse pour récupérer un district avec ses régions
/// </summary>
/// <param name="Id">L'identifiant unique du district</param>
/// <param name="Nom">Le nom du district</param>
/// <param name="Superficie">La superficie en kilomètres carrés</param>
/// <param name="Population">Le nombre d'habitants</param>
/// <param name="Regions">La liste des régions appartenant à ce district</param>
public record GetDistrictResponse(Guid Id, 
    string Nom, 
    decimal Superficie,
    int Population,
    IEnumerable<LocaliteDto> Regions);
