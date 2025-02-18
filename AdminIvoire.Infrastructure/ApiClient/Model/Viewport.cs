using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Viewport
{
    [JsonPropertyName("northeast")]
    public Location? Northeast { get; set; }
    [JsonPropertyName("southwest")]
    public Location? Southwest { get; set; }
}
