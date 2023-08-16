using DeepState.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Tests.TestData
{
    public class PanopticonServiceTestData
    {
        public static List<LoggedMessage> RequestJWTExpectedLoggedMessages = new List<LoggedMessage>
        {
            new LoggedMessage("====Starting JWT Request Function====", Serilog.Events.LogEventLevel.Information),
            new LoggedMessage("Is JWT Null? True", Serilog.Events.LogEventLevel.Information),
            new LoggedMessage("Requesting Panopticon JWT...", Serilog.Events.LogEventLevel.Information),
            new LoggedMessage("Panopticon JWT Status Code: OK", Serilog.Events.LogEventLevel.Information),
        };

        public static string ExpectedJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
    }
}
