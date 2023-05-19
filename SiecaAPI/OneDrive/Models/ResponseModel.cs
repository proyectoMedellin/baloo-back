using System.Text.Json.Serialization;

namespace SiecaAPI.OneDrive.Models;

internal class ResponseModel
{
    [JsonPropertyName("@odata.context")]
    public string ODataContext { get; set; }

    public ValueModel[] Value { get; set; }
}