using Azure.Core;
using Azure.Identity;

namespace SiecaAPI.OneDrive.Manager;

internal class TokenManager
{
    private readonly IConfiguration configuration;

    public TokenManager(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private static AccessToken Token { get; set; }

    internal async Task<string> GetTokenAsync(CancellationToken token)
    {
        if (Token.Token is null || Token.ExpiresOn < DateTimeOffset.Now)
        {
            await RefreshTokenAsync(token);
        }

        return Token.Token!;
    }

    private async Task RefreshTokenAsync(CancellationToken token)
    {
        var options = new OnBehalfOfCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        Func<DeviceCodeInfo, CancellationToken, Task> callback = (code, cancellation) =>
        {
            Console.WriteLine(code.Message);
            return Task.FromResult(0);
        };

        var deviceCodeCredential = new UsernamePasswordCredential(configuration["Secret:UserName"], 
            configuration["Secret:Password"], configuration["Secret:TenantId"], configuration["Secret:ClientId"], options); 

        var tokenRequestContext = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });

        Token = await deviceCodeCredential.GetTokenAsync(tokenRequestContext, token);
    }
}