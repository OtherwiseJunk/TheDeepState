using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
    public class ChannelModulePermissions
    {
        public ulong GuildId { get; set; }
        [Key]
        public ulong ChannelId { get;set; }
        public bool HungerGamesModule { get; set; }
        public bool JackboxModule { get; set; }
        public bool MalarkeyModule { get; set; }
        public bool ModTeamRequestsModule { get; set; }
        public bool OutOfContextModule { get; set; }
        public bool RPGModule { get; set; }
        public bool UserRecordModule { get; set; }
    }
}
