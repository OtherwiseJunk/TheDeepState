using DeepState.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Tests.TestData
{
    public static class FeedbackTestData
    {
        static FeedbackTestData()
        {
            RequestAllFeedbackExpectedLoggedMessages = new();
            RequestAllFeedbackExpectedLoggedMessages.AddRange(RequestJWTExpectedLoggedMessages);
            RequestAllFeedbackExpectedLoggedMessages.Add(new LoggedMessage("Received the list of all outstanding Feedback.", Serilog.Events.LogEventLevel.Information));
        }
        public static List<LoggedMessage> RequestJWTExpectedLoggedMessages = new List<LoggedMessage>
        {
            new LoggedMessage("Requesting Panopticon JWT...", Serilog.Events.LogEventLevel.Information),
            new LoggedMessage("Panopticon JWT Status Code: OK", Serilog.Events.LogEventLevel.Information),
        };

        public static List<LoggedMessage> RequestAllFeedbackExpectedLoggedMessages;
    }
}
