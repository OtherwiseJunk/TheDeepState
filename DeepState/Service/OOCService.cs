using DeepState.Utilities;
using Panopticon.Shared.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepState.Service
{
    public class OOCService : PanopticonService
    {
        public OOCService(HttpClient client, ILogger logger) : base(client, logger)
        {
        }

        public bool AddRecord(ulong reportingUserId, ulong guildId, string base64Image)
        {
            OOCItem item = new OOCItem
            {
                ReportingUserId = reportingUserId,
                DiscordGuildId = guildId,
                ImageUrl = base64Image,
                DateStored = DateTime.Now
            };

            using (HttpRequestMessage req = new(HttpMethod.Post, $"https://localhost:7118/ooc"))
            {
                req.AddJWTAuthorization(RequestJWT);
                string jsonString = JsonSerializer.Serialize(item);
                req.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            _log.Information("Successfully created new OOCRecord.");
                            return true;
                        default:
                            _log.Error("Received an unexpected response from Panopticon request for creating a new OOC Item.");
                            _log.Error($"{resp.StatusCode} response body: {resp.Content.ReadAsStringAsync().Result}");
                            return false;
                    }
                }
            }
        }
        public OOCItem? GetRandomRecord()
        {
            using (HttpRequestMessage req = new(HttpMethod.Get, $"https://panopticon.cacheblasters.com/ooc/rand"))
            {
                req.AddJWTAuthorization(RequestJWT);

                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            _log.Information("Successfully retrieved a random OOC Item.");
                            string oocJSON = resp.Content.ReadAsStringAsync().Result;
                            return JsonSerializer.Deserialize<OOCItem>(oocJSON, JsonOptions);
                        default:
                            _log.Error("Received an unexpected response from Panopticon retrieving a random OOC Item.");
                            return null;
                    }
                }
            }
        }
    }
}
