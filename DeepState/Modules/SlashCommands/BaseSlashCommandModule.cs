using DeepState.Interfaces;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules.SlashCommands
{
    public class BaseSlashCommandModule
    {
        public List<string> ManagedCommandNames { get; set; }

        public BaseSlashCommandModule(List<string> commandNames) {
            ManagedCommandNames = commandNames;
        }

        public bool IsSlashCommandManager(string commandName)
        {
            return ManagedCommandNames.Contains(commandName);
        }
    }
}
