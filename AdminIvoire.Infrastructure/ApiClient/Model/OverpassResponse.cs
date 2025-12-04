using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record OverpassResponse
{
    [JsonPropertyName("version")]
    public double? Version { get; set; }

    [JsonPropertyName("generator")]
    public string? Generator { get; set; }

    [JsonPropertyName("osm3s")]
    public Osm3s? Osm3s { get; set; }

    [JsonPropertyName("elements")]
    public List<OverpassElement> Elements { get; set; } = [];
}

public record Osm3s
{
    [JsonPropertyName("timestamp_osm_base")]
    public string? TimestampOsmBase { get; set; }

    [JsonPropertyName("copyright")]
    public string? Copyright { get; set; }
}

public record OverpassElement
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("lat")]
    public double? Lat { get; set; }

    [JsonPropertyName("lon")]
    public double? Lon { get; set; }

    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; set; } = [];
}

