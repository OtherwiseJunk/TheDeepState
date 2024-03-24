using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models.SlashCommands
{
    internal class SlashCommandWithOptions : SlashCommandInformation
    {
        public SlashCommandWithOptions(string name, string description, List<SlashCommandOptionBuilder> options)
        {
            Name = name;
            Description = description;
            DefaultPermission = true;
            Options = options;
            NameLocalizations = new Dictionary<string, string>();
            DescriptionLocalizations = new Dictionary<string, string>();
        }
    }
}
