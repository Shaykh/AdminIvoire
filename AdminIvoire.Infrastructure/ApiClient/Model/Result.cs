using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Result
{
    [JsonPropertyName("address_components")]
    public List<AddressComponent> AddressComponents { get; set; } = [];
    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }
    [JsonPropertyName("geometry")]
    public Geometry? Geometry { get; set; }
    [JsonPropertyName("place_id")]
    public string? PlaceId { get; set; }
    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = [];
}
