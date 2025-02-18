using System.Text.Json.Serialization;

namespace AdminIvoire.Infrastructure.ApiClient.Model;

public record GoogleCoordinateResponse
{
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; } = [];
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
