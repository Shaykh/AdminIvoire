namespace AdminIvoire.Application.Dto;

public record GetDistrictResponse(Guid Id, 
    string Nom, 
    decimal Superficie,
    int Population,
    IEnumerable<LocaliteDto> Regions);
