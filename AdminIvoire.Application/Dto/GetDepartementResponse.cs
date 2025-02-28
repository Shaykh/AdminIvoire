namespace AdminIvoire.Application.Dto;

public record GetDepartementResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid RegionId,
    string RegionNom,
    string DistrictNom,
    IEnumerable<LocaliteDto> SousPrefectures,
    CoordonneesGeographiquesDto CoordonneesGeographiques);
