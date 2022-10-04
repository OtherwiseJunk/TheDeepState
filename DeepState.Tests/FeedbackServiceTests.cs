using DeepState.Service;
using DeepState.Tests.Mocks;
using NUnit.Framework;
using Panopticon.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;

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
            //TODO: Mock httpClient to return values instead of hitting a live API.
            _client = new HttpClient();
            _loggerMock = new ILoggerMock();
            _service = new FeedbackService(_client, _loggerMock);
        }

        [TearDown]
        public void TearDown()
        {
            _loggerMock.events.Clear();
        }


        [Test]
        public void VerifyPanopticonServiceRequestJWTMethodReturnsAJWT()
        {
            string jwt = _service.RequestJWT();
            Assert.AreEqual(jwt.Split('.').Length, 3);
            Assert.AreEqual(_loggerMock.events.Count, 3);
        }

        [Test]
        public void VerifyPanopticonServiceGetAllFeedbackReturnsAListOfFeedback()
        {            
            List<Feedback> feedback = _service.GetAllFeedback();
            Assert.IsTrue(feedback.Count > 0);
            Assert.AreEqual(_loggerMock.events.Count, 4);
        }
    }
}
