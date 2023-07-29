using DartsDiscordBots.Utilities;
using DeepState.Extensions;
using Discord;
using Panopticon.Shared.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;

namespace DeepState.Services
{
    public class OOCService : PanopticonService
    {
        private string OOCCaptionFormat = "{0} Originally reported by {1}";
        private List<string> OOCQuipFormats = new List<string>
        {
            "Another Libcraft Banger.",
            "Still can't believe they said this...",
            "SMDH, really?",
            "Ok, friend, whatever you say.",
            "Ban them tbh.",
            "A Libcraft Classic.",
            "This awful take brought to you by Libcraft.",
            "They're a genius!",
            "Libcraft actually believes this.",
            "Yikes Sweety, let's unpack this...",
            "Yikes Sweaty, let's unpack this..."
        };
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

            using (HttpRequestMessage req = new(HttpMethod.Post, $"https://panopticon.cacheblasters.com/ooc"))
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

        internal EmbedBuilder BuildOOCEmbed(string title,IGuild triggeringGuild, IMessageChannel triggeringChannel, OOCItem pulledItem)
        {
            IGuildUser reportingUser = triggeringGuild.GetUserAsync(pulledItem.ReportingUserId, CacheMode.AllowDownload).Result;
            string reportingUsername = DDBUtils.GetDisplayNameForUser(reportingUser);

            byte[] nameHash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(reportingUsername));

            EmbedBuilder embed = new EmbedBuilder();
            if(pulledItem.ReportingUserId == 69)
            {
                embed.WithTitle($"{title}Submitted by OtherwiseJunk because they fucking BIFFED IT and had to import everything again. Point and laugh.");
            }
            else
            {
                embed.WithTitle($"{title}{String.Format(OOCCaptionFormat, OOCQuipFormats.GetRandom(), reportingUsername)}");
            }            
            embed.WithImageUrl(pulledItem.ImageUrl);
            embed.WithColor(new Color(nameHash[0], nameHash[1], nameHash[2]));
            embed.AddField("Date Stored", $"{pulledItem.DateStored.ToString("yyyy-MM-dd")} (yyyy-MM-dd)");
            embed.WithFooter($"{pulledItem.ItemID}");

            return embed;
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

        public OOCItem? GetSpecificRecord(int oocId)
        {
            using (HttpRequestMessage req = new(HttpMethod.Get, $"https://panopticon.cacheblasters.com/ooc/{oocId}"))
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

        public string[] GetAllURLs()
        {
            using (HttpRequestMessage req = new(HttpMethod.Get, $"https://panopticon.cacheblasters.com/ooc/allUrl"))
            {
                req.AddJWTAuthorization(RequestJWT);

                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            _log.Information("Successfully retrieved a random OOC Item.");
                            string oocJSON = resp.Content.ReadAsStringAsync().Result;
                            return JsonSerializer.Deserialize<string[]>(oocJSON, JsonOptions);
                        default:
                            _log.Error("Received an unexpected response from Panopticon retrieving the list of OOC Item URLs.");
                            return null;
                    }
                }
            }
        }

        public bool DeleteRecord(int id)
        {
            using (HttpRequestMessage req = new(HttpMethod.Delete, $"https://panopticon.cacheblasters.com/ooc/{id}"))
            {
                req.AddJWTAuthorization(RequestJWT);
                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            _log.Information($"Successfully deleted OOC Item {id}.");
                            return true;
                        default:
                            _log.Error($"Received an unexpected response from Panopticon request deleting OOC Item {id}.");
                            _log.Error($"{resp.StatusCode} response body: {resp.Content.ReadAsStringAsync().Result}");
                            return false;
                    }
                }
            }
        }
    }
}
