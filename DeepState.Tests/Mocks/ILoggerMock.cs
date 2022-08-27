using Serilog;
using Serilog.Events;
using System;

namespace DeepState.Tests.Mocks
{
    internal class ILoggerMock : ILogger
    {
        public void Write(LogEvent logEvent)
        {
            Console.WriteLine("log");
        }
    }
}
