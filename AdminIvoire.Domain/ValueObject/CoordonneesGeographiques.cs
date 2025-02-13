namespace AdminIvoire.Domain.ValueObject;

public record CoordonneesGeographiques
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}