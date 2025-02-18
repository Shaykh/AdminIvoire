using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Location
{
    [JsonPropertyName("lat")]
    public double? Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double? Longitude { get; set; }
}