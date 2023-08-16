using DeepState.Services;
using DeepState.Tests.Mocks;
using DeepState.Tests.TestData;
using Moq.Protected;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DeepState.Tests
{
    internal class PanopticonServiceTests
    {
        HttpClient _client { get; set; }
        PanopticonService _service { get; set; }
        ILoggerMock _loggerMock { get; set; }

        [OneTimeSetUp]
        public void TestsSetup()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent($"{{\"access_token\": \"{PanopticonServiceTestData.ExpectedJWT}\"}}")
               });
            _client = new HttpClient(mockMessageHandler.Object);
            _loggerMock = new();
            _service = new(_client, _loggerMock);
        }

        [Test]
        public void VerifyPanopticonServiceRequestJWTMethodLogsExpectedMessagesOnSuccess()
        {
            _ = _service.RequestJWT();
            Assert.That(_loggerMock.DoLoggedMessagesMatchExpected(PanopticonServiceTestData.RequestJWTExpectedLoggedMessages), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            _loggerMock.events.Clear();
        }
    }
}
