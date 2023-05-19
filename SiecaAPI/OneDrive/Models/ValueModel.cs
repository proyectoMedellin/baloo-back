using System.Text.Json.Serialization;

namespace SiecaAPI.OneDrive.Models;

internal class ValueModel
{
    [JsonPropertyName("@microsoft.graph.downloadUrl")]
    public string? DownloadUrl { get; set; }

    public string? Id { get; set; }

    public string? Name { get; set; }
}
