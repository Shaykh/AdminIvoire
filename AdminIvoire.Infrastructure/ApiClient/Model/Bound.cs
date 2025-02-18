using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record Bound
{
    [JsonPropertyName("northeast")]
    public Location? Northeast { get; set; }
    [JsonPropertyName("southwest")]
    public Location? Southwest { get; set; }
}
