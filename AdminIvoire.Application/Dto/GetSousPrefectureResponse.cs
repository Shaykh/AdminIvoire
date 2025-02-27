namespace AdminIvoire.Application.Dto;

public record GetSousPrefectureResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    IEnumerable<LocaliteDto> Villages,
    CoordonneesGeographiquesDto CoordonneesGeographiques);