using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Geometry
{
    [JsonPropertyName("bounds")]
    public Bound? Bounds { get; set; }
    [JsonPropertyName("location")]
    public Location? Location { get; set; }
    [JsonPropertyName("location_type")]
    public string? LocationType { get; set; }
    [JsonPropertyName("viewport")]
    public Viewport? Viewport { get; set; }
}
