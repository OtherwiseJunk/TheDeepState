using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models
{
    public class CurrentProgress
    {
        public int total { get; set; }
        public int current { get; set; }
        public string percent_complete { get; set; }
        public DateTime last_update { get; set; }
    }

    public class StoresClosedResponse
    {
        public List<string> stores { get; set; }
        public DateTime last_updates { get; set; }
        public CurrentProgress current_progress { get; set; }
    }
}
