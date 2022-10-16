using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Tests.Models
{
    public class LoggedMessage
    {
        public string Message { get; set; }
        public LogEventLevel Level { get; set; }

        public LoggedMessage(string msg, LogEventLevel lvl)
        {
            Message = msg;
            Level = lvl;
        }
    }
}
