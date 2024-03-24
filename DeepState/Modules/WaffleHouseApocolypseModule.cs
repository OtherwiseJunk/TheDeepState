using DartsDiscordBots.SlashCommandModules;
using DeepState.Models;
using DeepState.Services;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
    internal class WaffleHouseApocolypseModule : BaseSlashCommandModule, ISlashCommandModule
    {
        private const string ApocalypseCheckCommand = "apocalypse";
        private SlashCommandProperties ApocolypseCheckCommandProperties = new SlashCommandBuilder().WithName(ApocalypseCheckCommand).WithDescription("Queries https://wafflehouseindex.live to see if any wafflehouses are closed").Build();
        private WaffleHouseApocalypseService _service { get; set; }
        public WaffleHouseApocolypseModule(WaffleHouseApocalypseService service) : base(new List<string>{ ApocalypseCheckCommand }, "Waffle House Apocolypse")
        {
            _service = service;
        }

        public async Task HandleSocketSlashCommand(SocketSlashCommand command)
        {
            if (IsSlashCommandManager(command.CommandName))
            {
                _ = command.RespondAsync(embed: BuildApocolypseCheckEmbed());
            }
        }

        private Embed BuildApocolypseCheckEmbed()
        {
            EmbedBuilder builder = new();
            StoresClosedResponse response = _service.GetStoresClosed();
            string title = response.stores.Count == 0 ? "Kalm" : "PANIK! It's the Apocalypse!";
            builder.Title = title;
            builder.AddField("Waffle House Locations Closed", response.stores.Count);
            builder.AddField("Last Updated", $"<t:{((DateTimeOffset)response.last_updates).AddHours(-4).ToUnixTimeSeconds()}:R>");

            return builder.Build();
        }

        public async Task InstallModuleSlashCommands(IGuild guild, DiscordSocketClient client)
        {
            Console.WriteLine("Attempting to install Waffle House Apocalypse Module Slash Command...");
            if (guild == null && client == null)
            {
                throw new ArgumentNullException("Either the guild or the provided discord client must not be null.", null as Exception);
            }
            try
            {
                if (guild == null)
                {
                    Console.WriteLine("No Guild Provided, installing as Global command.");
                    var listCommand = await client.CreateGlobalApplicationCommandAsync(ApocolypseCheckCommandProperties);
                }
                else
                {
                    Console.WriteLine("Guild Provided, installing as Guild command.");
                    await guild.CreateApplicationCommandAsync(ApocolypseCheckCommandProperties);
                }
                Console.WriteLine("Waffle House Apocalypse Module Installation Success!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered an error installing Waffle House Apocalypse slash command.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
