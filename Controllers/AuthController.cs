using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;


namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfidentialClientApplication _confidentialClientApp;

        public AuthController()
        {
            var clientId = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID");
            var tenantId = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET");

            _confidentialClientApp = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            var result = await GetAccessTokenAsync();

            if (result != null)
            {
                return Ok(new { AccessToken = result.AccessToken });
            }

            return Unauthorized("Unable to get access token.");
        }

        private async Task<AuthenticationResult> GetAccessTokenAsync()
        {
            var scopes = new string[] { "https://graph.microsoft.com/.default openid profile offline_access" };

            var result = await _confidentialClientApp.AcquireTokenForClient(scopes)
                .ExecuteAsync();

            return result;
        }
    }
}
