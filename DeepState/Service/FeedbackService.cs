using DeepState.Extensions;
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
    public class FeedbackService : PanopticonService
    {
        public FeedbackService(HttpClient client, ILogger logger) : base(client, logger)
        {            
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
                req.Content = new StringContent($"\"{msg.Replace(Environment.NewLine, String.Empty)}\"", Encoding.UTF8, "application/json");
                using (HttpResponseMessage resp = _httpClient.SendAsync(req).Result)
                {
                    if (resp.StatusCode != HttpStatusCode.NoContent)
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
                            return JsonSerializer.Deserialize<Feedback>(resp.Content.ReadAsStringAsync().Result, JsonOptions);
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
