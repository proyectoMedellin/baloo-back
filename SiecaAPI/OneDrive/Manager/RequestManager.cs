using SiecaAPI.OneDrive.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SiecaAPI.OneDrive.Manager;

internal class RequestManager
{
    private readonly IConfiguration configuration;

    public RequestManager(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private async Task<HttpClient> GetAuthenticatedClientAsync(CancellationToken token)
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new TokenManager(configuration)
            .GetTokenAsync(token));

        return client;
    }

    internal async Task<ResponseModel?> GetFilesAsync(CancellationToken token)
    {
        using var client = await GetAuthenticatedClientAsync(token);

        using var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me/drive/root:/balanzas:/children", token);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync(token);

            return JsonSerializer.Deserialize<ResponseModel>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        return null;
    }

    internal async Task ReadFilesAsync(FileDataModel[] fileContainers, CancellationToken token)
    {
        using var client = await GetAuthenticatedClientAsync(token);

        foreach (var file in fileContainers)
        {
            using var response = await client.GetAsync(file.DownloadUrl, token);

            if (response.IsSuccessStatusCode)
            {
                file.Bytes = await response.Content.ReadAsByteArrayAsync(token);
                file.IsDownloaded = true;
            }
        }
    }

    internal async Task UpdateFilesAsync(FileDataModel[] fileContainers, CancellationToken token)
    {
        using var client = await GetAuthenticatedClientAsync(token);

        foreach (var file in fileContainers)
        {
            var json = JsonSerializer.Serialize(new { name = file.NewName }, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            using var content = new StringContent(json, Encoding.Default, "application/json");
            using var response = await client.PatchAsync("https://graph.microsoft.com/v1.0/me/drive/items/" + file.ItemId, content, token);

            if (response.IsSuccessStatusCode)
            {

            }
        }
    }
}