using DeepState.Tests.Models;
using Panopticon.Shared.Models;
using System.Collections.Generic;


namespace DeepState.Tests.TestData
{
    public static class FeedbackTestData
    {
        static FeedbackTestData()
        {
            RequestAllFeedbackExpectedLoggedMessages = new();
            RequestAllFeedbackExpectedLoggedMessages.AddRange(PanopticonServiceTestData.RequestJWTExpectedLoggedMessages);
            RequestAllFeedbackExpectedLoggedMessages.Add(new LoggedMessage("Received the list of all outstanding Feedback.", Serilog.Events.LogEventLevel.Information));
        }

        public static List<LoggedMessage> RequestAllFeedbackExpectedLoggedMessages;

        public static Feedback[] TestFeedback = new Feedback[] {  };
    }
}
