using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Location
{
    [JsonPropertyName("lat")]
    public double? Lat { get; set; }
    [JsonPropertyName("lng")]
    public double? Lng { get; set; }
}