using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Interfaces
{
    public interface ISlashCommandModule
    {
        public Task InstallModuleSlashCommands(IGuild? guild, IDiscordClient? client);
        public Task HandleSocketSlashCommand(SocketSlashCommand command);
    }
}
