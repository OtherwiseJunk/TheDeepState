using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Fizzler;

namespace DeepState.Services
{
    public class PanopticonService
    {
        public readonly HttpClient _httpClient;
        internal string Auth0ClientId { get; set; }
        internal string Auth0ClientSecret { get; set; }
        internal string Auth0Audience { get; set; }
        internal string Auth0GrantType { get; set; }
        internal string Auth0Scope { get; set; }
        internal ILogger _log { get; set; }
        private static string JWT { get; set; }

        internal JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public PanopticonService(HttpClient client, ILogger logger)
        {
            Auth0ClientId = Environment.GetEnvironmentVariable("AUTH0CLIENTID");
            Auth0ClientSecret = Environment.GetEnvironmentVariable("AUTH0CLIENTSECRET");
            Auth0Audience = Environment.GetEnvironmentVariable("AUTH0AUDIENCE");
            Auth0GrantType = Environment.GetEnvironmentVariable("AUTH0GRANTTYPE");
            Auth0Scope = Environment.GetEnvironmentVariable("AUTH0SCOPE");
            _httpClient = client;
            _log = logger;
        }

        public string RequestJWT()
        {
            _log.Information("====Starting JWT Request Function====");
            _log.Information($"Is JWT Null? {JWT is null}");

            JwtSecurityToken token = null;
            if (JWT is not null)
            {
                JwtSecurityTokenHandler tokenHandler = new();
                token = tokenHandler.ReadJwtToken(JWT);
                _log.Information($"JWT is valid until ${token.ValidTo.ToString("dd MMM HH:mm:ss")} and it is currently ${DateTime.UtcNow.ToString("dd MMM HH:mm:ss")}");
            }
            if (token is not null && token.ValidTo > DateTime.UtcNow)
            {
                _log.Information("Reusing existing JWT...");
                return JWT;
            }
            _log.Information("Requesting Panopticon JWT...");
            

            using (HttpRequestMessage msg = new(HttpMethod.Post, "https://dev-apsgkx34.us.auth0.com/oauth/token"))
            {
                msg.Headers.Add("Accept", MediaTypeNames.Application.Json);
                msg.Content = new StringContent($"{{\"client_id\":\"{Auth0ClientId}\",\"client_secret\":\"{Auth0ClientSecret}\",\"audience\":\"{Auth0Audience}\",\"grant_type\":\"{Auth0GrantType}\",\"scope\":\"{Auth0Scope}\"}}",
                                    Encoding.UTF8,
                                    "application/json");

                using (HttpResponseMessage resp = _httpClient.SendAsync(msg).Result)
                {
                    _log.Information($"Panopticon JWT Status Code: {resp.StatusCode}");
                    string jsonToken = JsonSerializer.Deserialize<JsonNode>(resp.Content.ReadAsStringAsync().Result)["access_token"].GetValue<string>();
                    JWT = jsonToken;
                    return jsonToken;
                }
            }
        }
    }
}
