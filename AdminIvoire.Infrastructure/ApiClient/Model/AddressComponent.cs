using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record AddressComponent
{
    [JsonPropertyName("long_name")]
    public string? LongName { get; set; }
    [JsonPropertyName("short_name")]
    public string? ShortName { get; set; }
    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = [];
}
