using NUnit.Framework;
using Serilog;
using Serilog.Events;
using System.Linq;
using System.Collections.Generic;
using DeepState.Tests.Models;

namespace DeepState.Tests.Mocks
{
    internal class ILoggerMock : ILogger
    {
        public List<LoggedMessage> events { get; set; } = new List<LoggedMessage>();
        public void Write(LogEvent logEvent)
        {
            events.Add(new LoggedMessage(logEvent.RenderMessage(), logEvent.Level));
        }

        public bool DoLoggedMessagesMatchExpected(List<LoggedMessage> expectedMessages)
        {
            bool success = true;
            success &= (expectedMessages.Count == events.Count);
            foreach(LoggedMessage message in expectedMessages)
            {
                LoggedMessage actualMessage = events.FirstOrDefault(e => e.Message == message.Message);
                success &= actualMessage != null;
                success &= actualMessage.Level == message.Level;
            }
            return success;
        }
    }
}
