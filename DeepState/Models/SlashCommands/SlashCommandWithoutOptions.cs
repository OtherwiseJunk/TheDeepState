using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models.SlashCommands
{
    public class SlashCommandWithoutOptions : SlashCommandInformation
    {
        public SlashCommandWithoutOptions(string name, string description)
        {
            Name = name;
            Description = description;
            DefaultPermission = true;
            Options = new List<SlashCommandOptionBuilder>();
            NameLocalizations = new Dictionary<string, string>();
            DescriptionLocalizations = new Dictionary<string, string>();
        }
    }
}
