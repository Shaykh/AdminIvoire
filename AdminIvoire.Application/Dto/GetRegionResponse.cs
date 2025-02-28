namespace AdminIvoire.Application.Dto;

public record GetRegionResponse(Guid Id,
    string Nom,
    decimal Superficie,
    int Population,
    Guid DistrictId,
    string DistrictNom,
    IEnumerable<LocaliteDto> Departements);
