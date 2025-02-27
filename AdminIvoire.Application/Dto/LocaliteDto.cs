namespace AdminIvoire.Application.Dto;

public record LocaliteDto(Guid Id, 
    string Nom, 
    decimal Superficie, 
    int Population, 
    CoordonneesGeographiquesDto? CoordonneesGeographiques);
