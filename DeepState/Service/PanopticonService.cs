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

namespace DeepState.Service
{
    public class PanopticonService
    {
        public readonly HttpClient _httpClient;
        private string Auth0ClientId { get; set; }
        private string Auth0ClientSecret { get; set; }
        private string Auth0Audience { get; set; }
        private string Auth0GrantType { get; set; }
        private string Auth0Scope { get; set; }
        private ILogger _log { get; set; }

        private JsonSerializerOptions JsonOptions = new JsonSerializerOptions
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
                    _log.Information($"Received json: {json}");
                    token = json["access_token"].GetValue<string>();
                }
            }

            return token;
        }

        public List<Feedback> GetAllFeedback()
        {
            List<Feedback> feedback;

            using (HttpRequestMessage msg = new(HttpMethod.Get, "https://panopticon.cacheblasters.com/Feedback"))
            {
                msg.AddJWTAuthorization(RequestJWT);

                using (HttpResponseMessage resp = _httpClient.SendAsync(msg).Result)
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        feedback = JsonSerializer.Deserialize<Feedback[]>(resp.Content.ReadAsStringAsync().Result, JsonOptions).ToList();
                        _log.Information("Received the list of all outstanding Feedback.");
                    }
                    else
                    {
                        feedback = new List<Feedback>();
                        _log.Error("Received a non-200 response from Panopticon request for creating feedback.");
                    }
                }
            }

            return feedback;
        }

        public void CreateFeedback(string msg, ulong reportingUser)
        {
            using (HttpRequestMessage req = new(HttpMethod.Post, $"https://panopticon.cacheblasters.com/Feedback?userId={reportingUser}"))
            {
                req.AddJWTAuthorization(RequestJWT);
                req.Content = new StringContent($"\"{msg.Replace(Environment.NewLine,String.Empty)}\"", Encoding.UTF8, "application/json");
                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    if(resp.StatusCode != HttpStatusCode.NoContent)
                    {
                        _log.Error("Received a non-200 response from Panopticon request for creating feedback.");
                        _log.Error($"Response Content: {resp.Content.ReadAsStringAsync().Result}");
                    }                    
                }
            }
        }

        public Feedback GetFeedback(int feedbackId)
        {
            using (HttpRequestMessage req = new(HttpMethod.Get, $"https://panopticon.cacheblasters.com/Feedback/{feedbackId}"))
            {
                req.AddJWTAuthorization(RequestJWT);

                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            _log.Information("Successfully found the single peice of feedback.");
                            return JsonSerializer.Deserialize<Feedback>(resp.Content.ReadAsStringAsync().Result);
                        case HttpStatusCode.NotFound:
                            _log.Error("Received a 404 response from Panopticon request for getting a single peice of feedback.");
                            return null;
                        default:
                            _log.Error("Received an unexpected response from Panopticon request for getting a single piece of feedback.");
                            return null;
                    }
                }
            }
        }
    }
}
