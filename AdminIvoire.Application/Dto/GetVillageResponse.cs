namespace AdminIvoire.Application.Dto;

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