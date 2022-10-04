using Panopticon.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serilog;
using DeepState.Utilities;
using System.Net;
using DeepState.Data.Models;

namespace DeepState.Service
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
            _log.Information("Requesting Panopticon JWT...");
            string token = "";

            using (HttpRequestMessage msg = new(HttpMethod.Post, "https://dev-apsgkx34.us.auth0.com/oauth/token"))
            {
                msg.Headers.Add("Accept", MediaTypeNames.Application.Json);
                msg.Content = new StringContent($"{{\"client_id\":\"{Auth0ClientId}\",\"client_secret\":\"{Auth0ClientSecret}\",\"audience\":\"{Auth0Audience}\",\"grant_type\":\"{Auth0GrantType}\",\"scope\":\"{Auth0Scope}\"}}",
                                    Encoding.UTF8,
                                    "application/json");

                using (HttpResponseMessage resp = _httpClient.SendAsync(msg).Result)
                {
                    _log.Information($"Panopticon JWT Status Code: {resp.StatusCode}");
                    JsonNode json = JsonSerializer.Deserialize<JsonNode>(resp.Content.ReadAsStringAsync().Result);
                    token = json["access_token"].GetValue<string>();
                }
            }
            return token;
        }
    }
}
