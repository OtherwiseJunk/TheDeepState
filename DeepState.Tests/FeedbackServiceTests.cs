using DeepState.Services;
using DeepState.Tests.Mocks;
using DeepState.Tests.TestData;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Panopticon.Shared.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Moq.Contrib.HttpClient;

namespace DeepState.Tests
{
    public class FeedbackServiceTests
    {
        HttpClient _client { get; set; }
        FeedbackService _service { get; set; }
        ILoggerMock _loggerMock { get; set; }

        [OneTimeSetUp]
        public void TestsSetup()
        {
            Mock<HttpMessageHandler> mockMessageHandler = new();
            string feedbackJson = $"[{JsonSerializer.Serialize(new Feedback(0, "message"))}]";
            mockMessageHandler.SetupRequest(HttpMethod.Post, "https://dev-apsgkx34.us.auth0.com/oauth/token").ReturnsResponse($"{{\"access_token\": \"{PanopticonServiceTestData.ExpectedJWT}\"}}");
            mockMessageHandler.SetupRequest(HttpMethod.Get, "https://panopticon.cacheblasters.com/Feedback").ReturnsResponse("[{\"id\": 23,\"reportingUser\": 882096305688702986,\"message\": \"<:smileyes:860720347828322304>\"}]");

            _client = new HttpClient(mockMessageHandler.Object);
            _loggerMock = new ILoggerMock();
            _service = new FeedbackService(_client, _loggerMock);
        }

        [TearDown]
        public void TearDown()
        {
            _loggerMock.events.Clear();
        }

        [Test]
        public void VerifyPanopticonServiceGetAllFeedbackLogsExpectedMessagesOnSuccess()
        {            
            _ = _service.GetAllFeedback();
            Assert.That(_loggerMock.DoLoggedMessagesMatchExpected(FeedbackTestData.RequestAllFeedbackExpectedLoggedMessages), Is.True);
        }
    }
}
