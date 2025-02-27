namespace AdminIvoire.Application.Dto;

public record GetDepartementResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    IEnumerable<LocaliteDto> SousPrefectures,
    CoordonneesGeographiquesDto CoordonneesGeographiques);
