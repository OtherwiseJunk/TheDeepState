using NUnit.Framework;
using Serilog;
using Serilog.Events;
using System.Linq;
using System.Collections.Generic;

namespace DeepState.Tests.Mocks
{
    internal class ILoggerMock : ILogger
    {
        public List<LogEvent> events { get; set; } = new List<LogEvent>();
        public void Write(LogEvent logEvent)
        {
            events.Add(logEvent);
        }
    }
}
